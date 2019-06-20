using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCreator : MonoBehaviour
{
    NodeCreation creator;
    List<TimeStamp> track = new List<TimeStamp>();
    public float[] timeValues;
    Movements moves = new Movements();

    void Start()
    {
        creator = gameObject.AddComponent<NodeCreation>();

        //Add Moves here
        //AddMove(new List<TimeStamp>(moves.RLRLRL(3)));
        //AddMove(new List<TimeStamp>(moves.RLRLRL2(10)));
        AddMove(moves.GetUkiDatas(@"C:\Users\lindi\Desktop\Movements\perso movement updated.csv", 5));

        /*//SHOWCASE
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
        //Go through the track and sees if a node must be spawned. If yes, spawns it and removes it from the list
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

    //Chooses the right constructor according to what is asked
    void spawnNode(TimeStamp ts){
        switch(ts.nodeType)
        {
            case 0 : 
                if (ts.specifiedPosition){
                    creator.CreateBasicNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.spawnPosition);
                } else {
                    if (ts.timeToFinish != 0)
                        creator.CreateBasicNode((NodeCreation.Joint) ts.joint, ts.timeToFinish);
                    else creator.CreateBasicNode((NodeCreation.Joint) ts.joint);
                }
                break;
            case 1 : 
                if (ts.specifiedPosition){
                    creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine, ts.spawnPosition, ts.pos1, ts.pos2);
                } else {
                    if (ts.timeToFinish != ts.defaultTimeToFinish || ts.timeLine != ts.defaultTimeLine){
                        creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine);
                    }
                    else creator.CreateLineNode((NodeCreation.Joint) ts.joint);
                }
                break; 
            case 2 : 
                if (ts.specifiedPosition){
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

