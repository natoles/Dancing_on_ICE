using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using DancingICE.Audio.BeatAnalysis;
using DancingICE.GameModes;

namespace DancingICE.RythmGame
{
    public abstract class RythmGameController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Mode to use if none is specified in RythmGameSettings")]
        private GameMode DefaultMode = null;

        [SerializeField]
        [Tooltip("AudioSource component that will be used for music playback")]
        private AudioSource audioPlayer = null;
        protected AudioSource AudioPlayer { get => audioPlayer; }

        [SerializeField]
        [Tooltip("The GameObject to show when loading the game")]
        private LoadingMusicScreen loadingScreen = null;

        [SerializeField]
        [Tooltip("The GameObject to load when pausing the game")]
        private PauseMenuController pauseMenu = null;

        protected SpectralFluxData SpectralFluxData { get; private set; } = null;
        protected List<SpectralFluxInfo> Peaks { get; private set; } = null;

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

        #region Pause Feature
        
        private bool paused = false;
        private float previousTimeScale = 1f;

        public void Pause()
        {
            if (!paused)
            {
                paused = true;
                previousTimeScale = Time.timeScale;
                audioPlayer.Pause();
                Time.timeScale = 0f;
                pauseMenu.gameObject.SetActive(true);
            }
        }

        public void Resume()
        {
            if (paused)
            {
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = previousTimeScale;
                audioPlayer.UnPause();
                paused = false;
            }
        }

        public void Retry()
        {
            if (paused)
            {
                Time.timeScale = previousTimeScale;
            }
            SceneHistory.ReloadActiveScene();
        }

        public void Quit()
        {
            if (paused)
            {
                Time.timeScale = previousTimeScale;
            }
            SceneHistory.LoadPreviousScene();
        }

        #endregion

        #region Audio Loading

        private Thread thread = null;
        private AudioClipData clipData = null;

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

                if (RythmGameSettings.GameMode.analyzeAudioSpectrum)
                {
                    if (SpectralFluxData == null)
                        SpectralFluxData = BeatAnalyzer.AnalyzeAudio(RythmGameSettings.BeatmapToLoad, clipData);

                    if (SpectralFluxData == null)
                        throw new NullReferenceException();

                    Peaks = SpectralFluxData.SelectPeaks(RythmGameSettings.Difficulty);
                }

                loaded = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                loadingException = e;
                loadingFailed = true;
            }
        }

        #endregion
        
        protected virtual void Awake()
        {
            if (RythmGameSettings.GameMode == null)
                RythmGameSettings.GameMode = DefaultMode;
        }

        protected virtual void Start()
        {
#if UNITY_EDITOR
            if (RythmGameSettings.BeatmapToLoad == null)
            {
                string path = BeatmapLoader.SelectAudioFile();
                if (string.IsNullOrEmpty(path))
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                RythmGameSettings.BeatmapToLoad = BeatmapLoader.CreateBeatmapFromAudio(path);
            }
#endif
            if (pauseMenu != null)
                pauseMenu.rythmGameController = this;

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

                    audioPlayer.clip = BeatmapLoader.CreateAudioClipFromData(clipData);
                    clipData = null;

                    OnLoaded();

                    audioPlayer.PlayDelayed(3f);

                    loadingScreen.Hide();
                }

                if (audioPlayer.isPlaying && audioPlayer.timeSamples > 0)
                {
                    if (!playbackStarted)
                    {
                        playbackStarted = true;
                        OnPlaybackStarted();
                    }

                    OnUpdate();
                }
                else if (playbackStarted && audioPlayer.timeSamples == 0) // end of playback
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