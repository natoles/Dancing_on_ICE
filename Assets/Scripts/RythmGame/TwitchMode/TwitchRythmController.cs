using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchRythmController : MonoBehaviour
{
    private BeatmapContainer bmc = null;
    private AudioSource player = null;
    private NodeCreation creator = null;
    private Camera mainCamera = null;

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

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        creator = new NodeCreation();
        mainCamera = Camera.main;
        bounds = mainCamera.OrthographicBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string path = BeatmapLoader.SelectBeatmapFile();
            if (path != null)
                bmc = BeatmapLoader.LoadBeatmapFile(path);
            player.clip = bmc.audio;
            player.PlayDelayed(3);
        }

        if (player.isPlaying && bmc != null)
        {
            bounds = mainCamera.OrthographicBounds();

            if (i1 < bmc.bm.Pool1.Count)
            {
                BeatTimestamp bts1 = bmc.bm.Pool1[i1];
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

            if (i2 < bmc.bm.Pool2.Count)
            {
                BeatTimestamp bts2 = bmc.bm.Pool2[i2];
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
