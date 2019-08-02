using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

namespace DancingICE.RythmGame
{
    public abstract class RythmGameController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioPlayer = null;
        protected AudioSource AudioPlayer { get => audioPlayer; }

        [SerializeField]
        private LoadingMusicScreen loadingScreen = null;
        
        private Thread thread = null;
        private AudioClipData clipData = null;

        protected SpectralFluxData SpectralFluxData { get; private set; } = null;
        protected List<SpectralFluxInfo> Peaks { get; private set; } = null;

        private bool loaded = false;
        private bool loadingFailed = false;
        private Exception loadingException = null;

        private bool playbackStarted = false;

        private void LoadBeatmapForPlay()
        {
            try
            {
                clipData = BeatmapLoader.LoadBeatmapAudio(RythmGameSettings.BeatmapToLoad);

                if (clipData == null)
                    throw new NullReferenceException();
                
                if (SpectralFluxData == null)
                    SpectralFluxData = BeatAnalyzer.AnalyzeAudio(RythmGameSettings.BeatmapToLoad, clipData);

                if (SpectralFluxData == null)
                    throw new NullReferenceException();

                Peaks = SpectralFluxData.SelectPeaks(RythmGameSettings.Difficulty);

                loaded = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                loadingException = e;
                loadingFailed = true;
            }
        }

        /// <summary>
        /// Invoked right after the audio have been loaded (and analyzed if the mode requires it)
        /// </summary>
        protected virtual void OnLoaded() { }

        /// <summary>
        /// Invoked if the audio couldn't be loaded (or analyzed, if the mode requires it)
        /// </summary>
        protected virtual void OnLoadingFailed(Exception e) { }

        /// <summary>
        /// Invoked each frame after the loading process have completed. (For general Update operations, please override the Update function instead)
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Invoked at the start of the audio playback
        /// </summary>
        protected virtual void OnPlaybackStarted() { }


        /// <summary>
        /// Invoked at the end of the audio playback
        /// </summary>
        protected virtual void OnPlaybackFinished() { }

        protected virtual void Start()
        {
#if UNITY_EDITOR
            if (RythmGameSettings.BeatmapToLoad == null)
                RythmGameSettings.BeatmapToLoad = BeatmapLoader.CreateBeatmapFromAudio(BeatmapLoader.SelectAudioFile());
#endif

            thread = new Thread(new ThreadStart(LoadBeatmapForPlay));
            thread.Start();
            loadingScreen.Text = System.IO.Path.GetFileNameWithoutExtension(RythmGameSettings.BeatmapToLoad?.sourceFile);
            loadingScreen.Show();
        }

        protected virtual void Update()
        {
            if (loaded)
            {
                if (thread != null)
                {
                    thread.Join();
                    thread = null;

                    AudioPlayer.clip = BeatmapLoader.CreateAudioClipFromData(clipData);
                    clipData = null;

                    OnLoaded();

                    AudioPlayer.PlayDelayed(3f);

                    loadingScreen.Hide();
                }

                if (AudioPlayer.isPlaying && AudioPlayer.timeSamples > 0)
                {
                    if (!playbackStarted)
                    {
                        playbackStarted = true;
                        OnPlaybackStarted();
                    }

                    OnUpdate();
                }
                else if (playbackStarted && AudioPlayer.timeSamples == 0) // end of playback
                {
                    OnPlaybackFinished();
                    SceneHistory.LoadPreviousScene();
                }
            }
            else if (loadingFailed)
            {
                OnLoadingFailed(loadingException);
                NotificationManager.Instance.PushNotification("Failed to load beatmap audio", Color.white, Color.red);
                SceneHistory.LoadPreviousScene();
            }
        }
    }
}