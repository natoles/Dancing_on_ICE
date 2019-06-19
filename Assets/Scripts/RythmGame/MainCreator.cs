using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCreator : MonoBehaviour
{
    NodeCreation creator;
    public GameObject self;
    List<TimeStamp> track = new List<TimeStamp>();
    public float[] timeValues;
    Movements moves = new Movements();

    void Start()
    {
        creator = self.AddComponent<NodeCreation>();

        AddMove(new List<TimeStamp>(moves.RLRLRL(3)));
        AddMove(new List<TimeStamp>(moves.RLRLRL(10)));

    }

    void Update()
    {
        int cpt = 0;
        while (cpt < track.Count){
            if (track[cpt].timeSpawn <= Time.time){
                spawnNode(track[cpt]);
                track.Remove(track[cpt]);
                cpt--;
            }
            cpt++;
        }
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
                    if (ts.timeToFinish != ts.defaultTimeToFinish || ts.timeLine != ts.defaultTimeLine){
                        creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine);
                    }
                    else creator.CreateLineNode((NodeCreation.Joint) ts.joint);
                }
                break; 
            case 2 : 
                if (ts.spawnPosition != Vector3.zero){
                    creator.CreateAngleNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.startAngle, ts.spawnPosition);
                } else {
                    if (ts.timeToFinish != 0)
                        creator.CreateAngleNode((NodeCreation.Joint) ts.joint, ts.timeToFinish);
                    else creator.CreateAngleNode((NodeCreation.Joint) ts.joint);
                }
                break;
            default :
                Debug.Log("PAS NORMAL");
                break;
        }
    }

    void AddMove(List<TimeStamp> move){
        for(int i = 0; i< move.Count; i++){
            track.Add(move[i]);
        }
    }
}

