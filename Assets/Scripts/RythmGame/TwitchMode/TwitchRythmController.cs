using System.Threading;
using UnityEngine;
using Kinect = Windows.Kinect;

public class TwitchRythmController : MonoBehaviour
{
    bool playbackStarted = false;

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

    private Thread loader = null;
    private AudioClipData clipData = null;
    private bool loaded = false;
    private bool loadingFailed = false;

    private void LoadBeatmapForPlay()
    {
        clipData = BeatmapLoader.LoadBeatmapAudio(BeatmapToLoad);

        loaded = clipData != null;
        loadingFailed = clipData == null;
    }

    #endregion

    #region Node Spawning

    private BeatAnalyzer analyzer = null;
    private bool analyzed = false;

    private NodeCreation creator = null;
    private Bounds bounds;

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

    private int previousSample = -1;
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
        
        loader = new Thread(new ThreadStart(LoadBeatmapForPlay));
        loader.Start();
        loadingScreen.Text = System.IO.Path.GetFileNameWithoutExtension(BeatmapToLoad?.sourceFile);
        loadingScreen.Show();
    }
    
    private void Update()
    {
        if (loaded)
        {
            if (loader != null)
            {
                loader.Join();
                loader = null;
                
                player.clip = BeatmapLoader.CreateAudioClipFromData(clipData);
                clipData = null;
                
                analyzer = new BeatAnalyzer(player, ThresholdMultiplier);
                analyzer.Start();
            }

            if (!analyzed)
            {
                if (analyzer.Completed)
                {
                    analyzed = true;
                    player.PlayDelayed(3);
                    loadingScreen.Hide();
                }
                else if (analyzer.Crashed)
                {
                    NotificationManager.Instance.PushNotification("Failed to analyze audio", Color.white, Color.red);
                    SceneHistory.LoadPreviousScene();
                }
            }

            if (player.isPlaying)
            {
                playbackStarted = true;

                bounds = mainCamera.OrthographicBounds();

                int currSample = analyzer.SampleIndex(player.time + ApproachTime);
                for (int i = previousSample + 1; i <= currSample && i < analyzer.SpectralFluxSamples.Count; ++i)
                {
                    if (Time.time > previousNodeSpawning + SpawnDelay && analyzer.SpectralFluxSamples[currSample].IsPeak(analyzer.ThresholdMultiplier))
                    {
                        previousNodeSpawning = Time.time;
                        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, ApproachTime, ComputePos(Kinect.JointType.HandLeft, previousNodeSpawning));
                    }
                }
                previousSample = currSample;

                //if (i1 < beatmapToLoad.bm.Pool1.Count)
                //{
                //    BeatTimestamp bts1 = beatmapToLoad.bm.Pool1[i1];
                //    if (player.time >= bts1.time - approachTime)
                //    {
                //        if (bts1.type == BeatType.Simple)
                //        {
                //            creator.CreateBasicNode(NodeCreation.Joint.LeftHand, approachTime, ComputePosLeft(Time.time));
                //        }
                //        else
                //        {
                //            LinkedList<Vector3> lnk = new LinkedList<Vector3>();
                //            for (float t = sliderPlotTime; t < bts1.duration; t += sliderPlotTime)
                //            {
                //                lnk.AddLast(ComputePosLeft(Time.time + t));
                //            }
                //            lnk.AddLast(ComputePosLeft(Time.time + bts1.duration));
                //            Vector3[] points = new Vector3[lnk.Count];
                //            lnk.CopyTo(points, 0);
                //            creator.CreateLineNode(NodeCreation.Joint.LeftHand, approachTime, bts1.duration, ComputePosLeft(Time.time), points);
                //        }
                //        i1++;
                //    }
                //}
            }
            else if (playbackStarted)
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
