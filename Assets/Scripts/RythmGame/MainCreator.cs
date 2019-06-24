using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCreator : MonoBehaviour
{
    public enum NodeType {BN = 0, LN = 1, AN = 2}
    public enum joint {H = 0, RH = 1, LH = 2}
    NodeCreation creator;
    List<TimeStamp> track = new List<TimeStamp>();
    public float[] timeValues;
    Movements moves = new Movements();

    void Start()
    {
        creator = new NodeCreation();
        string simpleMovePath = @"C:\Users\lindi\Desktop\Movements\Test2.csv";

        //Add Moves here
        //AddMove(new List<TimeStamp>(moves.RLRLRL(3)));
        //AddMove(new List<TimeStamp>(moves.RLRLRL2(10)));
        //AddMove(moves.GetUkiDatas(simpleMovePath,3,10,0,0, new TimeStamp(0,1,1,4f,25f,Vector3.zero,new Vector3[0])));
        AddMove(moves.GetUkiDatas(simpleMovePath,3,10,0,0, new TimeStamp(0,0,1,4f,Vector3.zero)));


    
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
                    creator.CreateBasicNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.spawnPosition);
                break;
            case 1 : 
                    creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine, ts.spawnPosition, ts.pathPositions);
                break; 
            case 2 : 
                    creator.CreateAngleNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.startAngle, ts.spawnPosition);
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

