using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchRythmController : MonoBehaviour
{
    BeatmapContainer bmc = null;
    AudioSource player = null;
    NodeCreation creator = null;

    float approachTime = 1f;
    Vector3 spawnLeft = new Vector3(-10, 0, 0);
    Vector3 spawnRight = new Vector3( 10, 0, 0);

    [SerializeField]
    float speed = 3f;

    int i1 = 0;
    int i2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        creator = new NodeCreation();
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
            if (i1 < bmc.bm.Pool1.Count)
            {
                BeatTimestamp bts1 = bmc.bm.Pool1[i1];
                if (player.time >= bts1.time - approachTime)
                {
                    if (bts1.type == BeatType.Simple)
                    {
                        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, approachTime, spawnLeft);
                    }
                    else
                    {
                        creator.CreateLineNode(NodeCreation.Joint.LeftHand, approachTime, bts1.duration, spawnLeft, new Vector3[] { spawnLeft + Vector3.up * 10f, spawnLeft + Vector3.up * 10f + Vector3.left * 10f });
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
                        creator.CreateBasicNode(NodeCreation.Joint.RightHand, approachTime, spawnRight);
                    }
                    else
                    {
                        creator.CreateLineNode(NodeCreation.Joint.RightHand, approachTime, bts2.duration, spawnRight, new Vector3[] { spawnRight + Vector3.up * 10f, spawnRight + Vector3.up * 10f + Vector3.right * 10f });
                    }
                    i2++;
                }
            }
        }
    }
}
