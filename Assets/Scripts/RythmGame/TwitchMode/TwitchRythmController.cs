using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TwitchRythmController : MonoBehaviour
{
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
    
    private static float difficulty = 1f;

    private Thread loader = null;
    private bool loaded = false;
    private bool loading = false;
    private bool selecting = false;
    private AudioClipData clipData = null;

    private BeatThreadedAnalyser analyser = null;

    private AudioSource player = null;
    private NodeCreation creator = null;
    private Camera mainCamera = null;

    [SerializeField]
    private LoadingMusicScreen loadingScreen = null;

    private Bounds bounds;
    private readonly float sliderPlotTime = 0.01f;
    private readonly float bx = 0.225f;
    private readonly float dx = 0.150f;
    private readonly float by = 0.275f;
    private readonly float dy = 0.375f;

    private readonly float approachTime = 1.2f;

    private readonly float baseSpeed = 2f;
    private readonly float scalingSpeed = 0.375f;

    private readonly float noteSpawningDelay = 0.5f;

    private int previousSample = -1;
    private float previousNodeSpawning = float.MinValue;

    private Vector3 ComputePosLeft(float time)
    {
        return new Vector3(
            bounds.center.x - (bx + dx * Mathf.Cos(time * (baseSpeed + (difficulty - 1) * scalingSpeed))) * bounds.extents.x,
            bounds.center.y + (by + dy * Mathf.Sin(time * (baseSpeed + (difficulty - 1) * scalingSpeed))) * bounds.extents.y
            );
    }

    private Vector3 ComputePosRight(float time)
    {
        return new Vector3(
            bounds.center.x + (bx + dx * Mathf.Cos(time * (baseSpeed + (difficulty - 1) * scalingSpeed))) * bounds.extents.x,
            bounds.center.y + (by + dy * Mathf.Sin(time * (baseSpeed + (difficulty - 1) * scalingSpeed))) * bounds.extents.y
            );
    }

    private void LoadBeatmapForPlay()
    {
        clipData = BeatmapLoader.LoadBeatmapAudio(BeatmapToLoad);

        if (clipData != null)
            loaded = true;
        else
            NotificationManager.Instance.PushNotification("Failed to load beatmap audio", Color.white, Color.red);

        loading = false;
    }

    private void SelectedBeatmapAsyncHandler(string[] files)
    {
        if (files.Length == 0 || files[0] == null || files[0] == "")
        {
            selecting = false;
            return;
        }

        BeatmapToLoad = BeatmapLoader.LoadBeatmapFile(files[0]);
        selecting = false;
    }

    private void Start()
    {
        player = GetComponent<AudioSource>();
        creator = new NodeCreation();
        mainCamera = Camera.main;
        bounds = mainCamera.OrthographicBounds();
    }
    
    private void FixedUpdate()
    {
        Debug.Log(player.time);
        if (!loaded)
        {
            if (BeatmapToLoad == null)
            {
                if (!selecting && Time.time > 0.1f)
                {
                    selecting = true;
                    BeatmapLoader.SelectBeatmapFileAsync(SelectedBeatmapAsyncHandler);
                    loadingScreen.Display();
                }
            }
            else if (!loading)
            {
                loading = true;
                loader = new Thread(new ThreadStart(LoadBeatmapForPlay));
                loader.Start();
                loadingScreen.Display(System.IO.Path.GetFileNameWithoutExtension(BeatmapToLoad?.sourceFile));
            }
        }
        else
        {
            if (loader != null)
            {
                loader.Join();
                loader = null;
                
                player.clip = BeatmapLoader.CreateAudioClipFromData(clipData);
                player.PlayDelayed(3);
                clipData = null;

                analyser = new BeatThreadedAnalyser(player);
                analyser.Start();

                loadingScreen.Hide();
            }

            if (player.isPlaying && BeatmapToLoad != null)
            {
                bounds = mainCamera.OrthographicBounds();
                
                int currSample = analyser.SampleIndex(player.time + approachTime);
                for (int i = previousSample + 1; i <= currSample && i < analyser.SpectralFluxSamples.Count; ++i)
                {
                    if (Time.time > previousNodeSpawning + noteSpawningDelay && analyser.SpectralFluxSamples[currSample].isPeak)
                    {
                        previousNodeSpawning = Time.time;
                        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, approachTime, ComputePosLeft(previousNodeSpawning));
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

                //if (i2 < beatmapToLoad.bm.Pool2.Count)
                //{
                //    BeatTimestamp bts2 = beatmapToLoad.bm.Pool2[i2];
                //    if (player.time >= bts2.time - approachTime)
                //    {
                //        if (bts2.type == BeatType.Simple)
                //        {
                //            creator.CreateBasicNode(NodeCreation.Joint.RightHand, approachTime, ComputePosRight(Time.time));
                //        }
                //        else
                //        {
                //            LinkedList<Vector3> lnk = new LinkedList<Vector3>();
                //            for (float t = sliderPlotTime; t < bts2.duration; t += sliderPlotTime)
                //            {
                //                lnk.AddLast(ComputePosRight(Time.time + t));
                //            }
                //            lnk.AddLast(ComputePosRight(Time.time + bts2.duration));
                //            Vector3[] points = new Vector3[lnk.Count];
                //            lnk.CopyTo(points, 0);
                //            creator.CreateLineNode(NodeCreation.Joint.RightHand, approachTime, bts2.duration, ComputePosRight(Time.time), points);
                //        }
                //        i2++;
                //    }
                //}
            }
        }
    }
}
