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

        track.Add(new TimeStamp(4,0,0));
        track.Add(new TimeStamp(4,0,1));
        track.Add(new TimeStamp(4,0,2));

        track.Add(new TimeStamp(8,1,1));
        track.Add(new TimeStamp(8,1,2));

        track.Add(new TimeStamp(18, 0, 0, 5, new Vector3(0.1f,0,0)));
        track.Add(new TimeStamp(18, 0, 1, 4, new Vector3(5,0,0)));
        track.Add(new TimeStamp(18, 0, 2, 6, new Vector3(-5,0,0)));
        track.Add(new TimeStamp(18, 0, 2, 0.5f, Vector3.zero));

        track.Add(new TimeStamp(22, 1, 1, 2, 2, Vector3.zero, Vector3.zero, Vector3.zero));
        track.Add(new TimeStamp(22, 1, 2, 2, 2, Vector3.zero, Vector3.zero, Vector3.zero));
        track.Add(new TimeStamp(22, 1, 1, 2, 5, new Vector3(0,5,0), new Vector3(0,8,0), new Vector3(5,8,0)));

        
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
