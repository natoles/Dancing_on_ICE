using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TwitchRythmController : MonoBehaviour
{
    public static BeatmapContainer beatmapToLoad = null;
    private Thread loader = null;
    private bool loaded = false;
    private bool loading = false;
    private AudioClipData clipData = null;
    
    private AudioSource player = null;
    private NodeCreation creator = null;
    private Camera mainCamera = null;

    [SerializeField]
    private LoadingMusicScreen loadingScreen = null;

    private Bounds bounds;
    private float sliderPlotTime = 0.01f;
    private float bx = 0.225f;
    private float dx = 0.150f;
    private float by = 0.275f;
    private float dy = 0.375f;
    private float approachTime = 1f;
    private float speed = 2f;

    private int i1 = 0;
    private int i2 = 0;

    private Vector3 ComputePosLeft(float time)
    {
        return new Vector3(
            bounds.center.x - (bx + dx * Mathf.Cos(time * speed)) * bounds.extents.x,
            bounds.center.y + (by + dy * Mathf.Sin(time * speed)) * bounds.extents.y
            );
    }

    private Vector3 ComputePosRight(float time)
    {
        return new Vector3(
            bounds.center.x + (bx + dx * Mathf.Cos(time * speed)) * bounds.extents.x,
            bounds.center.y + (by + dy * Mathf.Sin(time * speed)) * bounds.extents.y
            );
    }

    private void LoadBeatmapForPlay()
    {
        if (beatmapToLoad == null)
        {
            string path = BeatmapLoader.SelectBeatmapFile();
            if (path == null || path == "")
                return;
            beatmapToLoad = BeatmapLoader.LoadBeatmapFile(path);
        }

        clipData = BeatmapLoader.LoadBeatmapAudio(beatmapToLoad);

        loaded = true;
        loading = false;
    }

    private void Start()
    {
        player = GetComponent<AudioSource>();
        creator = new NodeCreation();
        mainCamera = Camera.main;
        bounds = mainCamera.OrthographicBounds();
    }
    
    private void Update()
    {
        if (!loaded)
        {
            if (!loading)
            {
                loading = true;
                loader = new Thread(new ThreadStart(LoadBeatmapForPlay));
                loader.Start();
                loadingScreen.Display(System.IO.Path.GetFileNameWithoutExtension(beatmapToLoad.sourceFile));
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

                loadingScreen.Hide();
            }

            if (player.isPlaying && beatmapToLoad != null)
            {
                bounds = mainCamera.OrthographicBounds();

                if (i1 < beatmapToLoad.bm.Pool1.Count)
                {
                    BeatTimestamp bts1 = beatmapToLoad.bm.Pool1[i1];
                    if (player.time >= bts1.time - approachTime)
                    {
                        if (bts1.type == BeatType.Simple)
                        {
                            creator.CreateBasicNode(NodeCreation.Joint.LeftHand, approachTime, ComputePosLeft(Time.time));
                        }
                        else
                        {
                            LinkedList<Vector3> lnk = new LinkedList<Vector3>();
                            for (float t = sliderPlotTime; t < bts1.duration; t += sliderPlotTime)
                            {
                                lnk.AddLast(ComputePosLeft(Time.time + t));
                            }
                            lnk.AddLast(ComputePosLeft(Time.time + bts1.duration));
                            Vector3[] points = new Vector3[lnk.Count];
                            lnk.CopyTo(points, 0);
                            creator.CreateLineNode(NodeCreation.Joint.LeftHand, approachTime, bts1.duration, ComputePosLeft(Time.time), points);
                        }
                        i1++;
                    }
                }

                if (i2 < beatmapToLoad.bm.Pool2.Count)
                {
                    BeatTimestamp bts2 = beatmapToLoad.bm.Pool2[i2];
                    if (player.time >= bts2.time - approachTime)
                    {
                        if (bts2.type == BeatType.Simple)
                        {
                            creator.CreateBasicNode(NodeCreation.Joint.RightHand, approachTime, ComputePosRight(Time.time));
                        }
                        else
                        {
                            LinkedList<Vector3> lnk = new LinkedList<Vector3>();
                            for (float t = sliderPlotTime; t < bts2.duration; t += sliderPlotTime)
                            {
                                lnk.AddLast(ComputePosRight(Time.time + t));
                            }
                            lnk.AddLast(ComputePosRight(Time.time + bts2.duration));
                            Vector3[] points = new Vector3[lnk.Count];
                            lnk.CopyTo(points, 0);
                            creator.CreateLineNode(NodeCreation.Joint.RightHand, approachTime, bts2.duration, ComputePosRight(Time.time), points);
                        }
                        i2++;
                    }
                }
            }
        }
    }
}
