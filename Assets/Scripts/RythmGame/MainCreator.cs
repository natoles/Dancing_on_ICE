using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCreator : MonoBehaviour
{
    

    NodeCreation creator;
    public GameObject self;
    List<TimeStamp> track = new List<TimeStamp>();

    void Start()
    {
        creator = self.AddComponent<NodeCreation>();
        /* 
        creator.CreateBasicNode(NodeCreation.Joint.Hand, 10);
        creator.CreateBasicNode(NodeCreation.Joint.RightHand, 9, new Vector3(-6,-6,0));
        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, 5);
        creator.CreateBasicNode(NodeCreation.Joint.Hand);*/

        //creator.CreateLineNode(NodeCreation.Joint.RightHand);
        //creator.CreateLineNode(NodeCreation.Joint.LeftHand, 2, 2,
        //     new Vector3(-8,-8,0), new Vector3(-2,-8,0), new Vector3(-2,-5,0));
        //creator.CreateLineNode(NodeCreation.Joint.RightHand, 8, 8);

        //track.Add(new TimeStamp(5, 0, 0, 2, new Vector3(2,2,0)));
        //track.Add(new TimeStamp(5, 0, 1, 2, Vector3.zero));
        //track.Add(new TimeStamp(7, 0, 0, 6, Vector3.zero));
        //track.Add(new TimeStamp(9, 0, 2, 1, Vector3.zero));
        track.Add(new TimeStamp(5, 1, 1, 2, 5, Vector3.zero, Vector3.zero, Vector3.zero));

    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i< track.Count; i++){
            if (track[i].timeSpawn - track[i].timeToFinish <= Time.time){
                spawnNode(track[i]);
                track.Remove(track[i]);
            }
        }
    }

    void sendD(int b){
        Debug.Log(b);
    }

    void spawnNode(TimeStamp ts){
        switch(ts.nodeType)
        {
            case 0 : 
                if (ts.spawnPosition != Vector3.zero){
                    creator.CreateBasicNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.spawnPosition);
                } else {
                    if (ts.timeToFinish != 0)
                        creator.CreateBasicNode((NodeCreation.Joint) ts.joint, ts.timeToFinish);
                    else creator.CreateBasicNode((NodeCreation.Joint) ts.joint);
                }
                break;
            case 1 : 
                if (ts.spawnPosition != Vector3.zero){
                    creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine, ts.spawnPosition, ts.pos1, ts.pos2);
                } else {
                    if (ts.timeToFinish != 0){
                        Debug.Log("Hey");
                        creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine);
                    }
                    else creator.CreateLineNode((NodeCreation.Joint) ts.joint);
                }
                break; 
            default :
                Debug.Log("PAS NORMAL");
                break;
        }
    }
}
