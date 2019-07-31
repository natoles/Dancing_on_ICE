using System.Collections.Generic;
using System.Threading;
using System.IO;
using UnityEngine;
using Kinect = Windows.Kinect;
using Newtonsoft.Json;

public class TwitchRythmController : MonoBehaviour
{
    #region Properties

    public static BeatmapContainer BeatmapToLoad { set; get; } = null;

    public static float Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = Mathf.Clamp(value, 1, 5);
        }
    }

    private static float difficulty = 5f;

    #endregion

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
    private BeatAnalyzer analyzer = null;
    private SpectralFluxData spectralFluxData = null;
    private List<SpectralFluxInfo> peaks = null;
    private bool loaded = false;
    private bool loadingFailed = false;

    private void AnalyzeAudio(AudioClipData clipData)
    {
        analyzer = new BeatAnalyzer(clipData);
        spectralFluxData = analyzer.GetFullSpectrum();
        analyzer = null;

        StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/SpectrumData/" + BeatmapToLoad.sourceFile + ".json");
        writer.Write(JsonConvert.SerializeObject(spectralFluxData));
        writer.Close();
    }

    private void LoadSpectrumFromFile()
    {
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/SpectrumData/" + BeatmapToLoad.sourceFile + ".json");
        spectralFluxData = JsonConvert.DeserializeObject<SpectralFluxData>(reader.ReadToEnd());
        reader.Close();
    }

    private void LoadBeatmapForPlay()
    {
        clipData = BeatmapLoader.LoadBeatmapAudio(BeatmapToLoad);

        if (clipData != null)
        {
            if (!new DirectoryInfo(Application.streamingAssetsPath + "/SpectrumData/").Exists)
                Directory.CreateDirectory(Application.streamingAssetsPath + "/SpectrumData/");

            if (new FileInfo(Application.streamingAssetsPath + "/SpectrumData/" + BeatmapToLoad.sourceFile + ".json").Exists)
                LoadSpectrumFromFile();
            else
                AnalyzeAudio(clipData);
            peaks = spectralFluxData.SelectPeaks(Difficulty);
        }

        loaded = clipData != null;
        loadingFailed = clipData == null;
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
    private readonly float maxApproachTime = 0.7f;
    private float ApproachTime { get { return Mathf.Lerp(minApproachTime, maxApproachTime, (difficulty - 1) / 5); } }

    private readonly float minSpeed = 1.5f;
    private readonly float maxSpeed = 3f;
    private float Speed { get { return Mathf.Lerp(minSpeed, maxSpeed, (difficulty - 1) / 5); } }

    private readonly float minSpawnDelay = 0.5f;
    private readonly float maxSpawnDelay = 0.15f;
    private float SpawnDelay { get { return Mathf.Lerp(minSpawnDelay, maxSpawnDelay, (difficulty - 1) / 5); } }

    private readonly float minThresholdMultiplier = 1.8f;
    private readonly float maxThresholdMultiplier = 1.5f;
    private float ThresholdMultiplier { get { return Mathf.Lerp(minThresholdMultiplier, maxThresholdMultiplier, (difficulty - 1) / 5); } }

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
        if (BeatmapToLoad == null)
            BeatmapToLoad = BeatmapLoader.CreateBeatmapFromAudio(BeatmapLoader.SelectAudioFile());
#endif
        
        thread = new Thread(new ThreadStart(LoadBeatmapForPlay));
        thread.Start();
        loadingScreen.Text = System.IO.Path.GetFileNameWithoutExtension(BeatmapToLoad?.sourceFile);
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

                analyzer = null;

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
