using System.Collections.Generic;
using System.Threading;
using System.IO;
using UnityEngine;
using Kinect = Windows.Kinect;
using Newtonsoft.Json;

public class TwitchRythmController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private AudioSource player = null;

    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField]
    private LoadingMusicScreen loadingScreen = null;

    #endregion

    #region Music Loading

    private Thread thread = null;
    private AudioClipData clipData = null;
    private SpectralFluxData spectralFluxData = null;
    private List<SpectralFluxInfo> peaks = null;
    private bool loaded = false;
    private bool loadingFailed = false;

    private void LoadBeatmapForPlay()
    {
        clipData = BeatmapLoader.LoadBeatmapAudio(RythmGameSettings.BeatmapToLoad);

        if (clipData != null)
        {
            if (spectralFluxData == null)
                spectralFluxData = BeatAnalyzer.AnalyzeAudio(RythmGameSettings.BeatmapToLoad, clipData);

            if (spectralFluxData != null)
                peaks = spectralFluxData.SelectPeaks(RythmGameSettings.Difficulty);
        }

        loaded = clipData != null && spectralFluxData != null && peaks != null;
        loadingFailed = !loaded;
    }

    #endregion

    #region Node Spawning

    private NodeCreation creator = null;
    private Bounds bounds = default;

    //private readonly float sliderPlotTime = 0.01f;
    private readonly float bx = 0.225f;
    private readonly float dx = 0.150f;
    private readonly float by = 0.275f;
    private readonly float dy = 0.375f;

    private readonly float minApproachTime = 2f;
    private readonly float maxApproachTime = 0.8f;
    private float ApproachTime { get { return Mathf.Lerp(minApproachTime, maxApproachTime, RythmGameSettings.DifficultyPercentage); } }

    private readonly float minSpeed = 1.75f;
    private readonly float maxSpeed = 3.5f;
    private float Speed { get { return Mathf.Lerp(minSpeed, maxSpeed, RythmGameSettings.DifficultyPercentage); } }

    private readonly float minSpawnDelay = 0.75f;
    private readonly float maxSpawnDelay = 0.15f;
    private float SpawnDelay { get { return Mathf.Lerp(minSpawnDelay, maxSpawnDelay, RythmGameSettings.DifficultyPercentage); } }

    private int currPeak = 0;
    private float previousNodeSpawning = float.MinValue;

    private Vector3 ComputePos(Kinect.JointType joint, float time)
    {
        return new Vector3(
            bounds.center.x + (joint == Kinect.JointType.HandLeft ? -1f : 1f) * (bx + dx * Mathf.Cos(time * Speed)) * bounds.extents.x,
            bounds.center.y + (                     1                       ) * (by + dy * Mathf.Sin(time * Speed)) * bounds.extents.y
            );
    }

    #endregion

  private void Start()
    {
        creator = new NodeCreation();
        bounds = mainCamera.OrthographicBounds();

#if UNITY_EDITOR
        if (RythmGameSettings.BeatmapToLoad == null)
            RythmGameSettings.BeatmapToLoad = BeatmapLoader.CreateBeatmapFromAudio(BeatmapLoader.SelectAudioFile());
#endif
        
        thread = new Thread(new ThreadStart(LoadBeatmapForPlay));
        thread.Start();
        loadingScreen.Text = System.IO.Path.GetFileNameWithoutExtension(RythmGameSettings.BeatmapToLoad?.sourceFile);
        loadingScreen.Show();
    }
    
    private void Update()
    {
        if (loaded)
        {
            if (thread != null)
            {
                thread.Join();
                thread = null;

                player.clip = BeatmapLoader.CreateAudioClipFromData(clipData);
                clipData = null;

                currPeak = 0;

                player.PlayDelayed(3f);

                loadingScreen.Hide();
            }

            if (player.isPlaying && player.time > 0)
            {
                bounds = mainCamera.OrthographicBounds();
                
                while (currPeak < peaks.Count && peaks[currPeak].time <= player.time + ApproachTime)
                {
                    if (Time.time > previousNodeSpawning + SpawnDelay)
                    {
                        previousNodeSpawning = Time.time;
                        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, peaks[currPeak].time - player.time, ComputePos(Kinect.JointType.HandLeft, previousNodeSpawning));
                    }
                    currPeak++;
                }
            }
            else if (player.timeSamples > player.clip.samples) // end of playback
            {
                SceneHistory.LoadPreviousScene();
            }
        }
        else if (loadingFailed)
        {
            NotificationManager.Instance.PushNotification("Failed to load beatmap audio", Color.white, Color.red);
            SceneHistory.LoadPreviousScene();
        }
    }
}
