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
    Movements moves;
    List<nbTime> nTs = new List<nbTime>();

    public class nbTime
    {
        public int nbNodes;
        public float time;

        public nbTime(int nbNodes1, float time1){
            nbNodes = nbNodes1;
            time = time1;
        }
    }


    void Start()
    {
        moves = GetComponent<Movements>();
        creator = self.AddComponent<NodeCreation>();

        AddMove(new List<TimeStamp>(moves.RLRLRL), 2);
        AddMove(new List<TimeStamp>(moves.RLRLRL), 7);
        

        /*  //SHOWCASE
        timeValues = new float[] {3,3,3, 7,7, 11,11,11, 18,18,18,18, 22,22,22, 29,29,29,29};

        track.Add(new TimeStamp(0,0,0));
        track.Add(new TimeStamp(0,0,1));
        track.Add(new TimeStamp(0,0,2));

        track.Add(new TimeStamp(0,1,1));
        track.Add(new TimeStamp(0,1,2));

        track.Add(new TimeStamp(0,2,0));
        track.Add(new TimeStamp(0,2,1));
        track.Add(new TimeStamp(0,2,2));

        track.Add(new TimeStamp(0, 0, 0, 5, new Vector3(0.1f,0,0)));
        track.Add(new TimeStamp(0, 0, 1, 4, new Vector3(5,0,0)));
        track.Add(new TimeStamp(0, 0, 2, 6, new Vector3(-5,0,0)));
        track.Add(new TimeStamp(0, 0, 2, 0.5f, Vector3.zero));

        track.Add(new TimeStamp(0, 1, 1, 2, 2, Vector3.zero, Vector3.zero, Vector3.zero));
        track.Add(new TimeStamp(0, 1, 2, 2, 2, Vector3.zero, Vector3.zero, Vector3.zero));
        track.Add(new TimeStamp(0, 1, 1, 2, 5, new Vector3(0,5,0), new Vector3(0,8,0), new Vector3(5,8,0)));

        track.Add(new TimeStamp(0, 2, 0, 5, 90, new Vector3(0.1f,0,0)));
        track.Add(new TimeStamp(0, 2, 1, 4, 0, new Vector3(5,0,0)));
        track.Add(new TimeStamp(0, 2, 2, 6, 180, new Vector3(-5,0,0)));
        track.Add(new TimeStamp(0, 2, 2, 0.5f, 0, Vector3.zero));
        
        if (track.Count == timeValues.Length)
        {
            for(int i=0; i< timeValues.Length; i++){
                track[i].timeSpawn = timeValues[i];
            }
        } else Debug.Log ("ERROR : track.Count != timeValues.Length");
        */

    }

    void Update()
    {
        if (track.Count > 0){
            if (Time.time >= nTs[0].time + track[0].timeSpawn ){
                spawnNode(track[0]);
                track.Remove(track[0]);
                nTs[0].nbNodes -= 1;
                if (nTs[0].nbNodes == 0){
                    nTs.Remove(nTs[0]);
                }
            }
        }

        /* 
        int cpt = 0;
        while (cpt < track.Count){
            if (track[cpt].timeSpawn <= Time.time){
                spawnNode(track[cpt]);
                track.Remove(track[cpt]);
                cpt--;
            }
            cpt++;
        }*/
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

    void AddMove(List<TimeStamp> move, int time){
        nbTime nT = new nbTime(move.Count, time); 
        nTs.Add(nT);

        for(int i = 0; i< move.Count; i++){
            track.Add(move[i]);
        }
    }

}

