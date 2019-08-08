using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to store all informations of a node 
public class TimeStamp
{
    public float timeSpawn;
    public int nodeType; //Basic=0; Line=1; Angle=2; Hold=3
    public int joint; //RightHand=0; LeftHand=1
    public float timeToFinish; //Time between apparition and disparition of teh node
    public float timeLine; //Time duration of the line node
    public float startAngle; //Angle of the angle node
    public float timeHold; //Hold time of the hold node
    public Vector3 spawnPosition; 
    public Vector3[] pathPositions; //Position of the points forming the line node

    //Simplified constructor(default values)
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
    } 


    //BasicNode Constructor
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, Vector3 spawnPosition1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        spawnPosition = spawnPosition1;
    }  

    //LineNode Constructor
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, float timeLine1, Vector3 spawnPosition1, Vector3[] pathPositions1){
        nodeType = nodeType1;
        joint = joint1;
        timeLine = timeLine1;
        pathPositions = pathPositions1;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        spawnPosition = spawnPosition1;
    }  

    //AngleNode and Holdnode Constructor 
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, float startAngleTimeHold, Vector3 spawnPosition1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        startAngle = startAngleTimeHold;
        timeHold = startAngleTimeHold;
        spawnPosition = spawnPosition1;
    }  

    public TimeStamp DeepCopyTS(TimeStamp ts1){
        TimeStamp ts2 = new TimeStamp(ts1.timeSpawn, ts1.nodeType, ts1.joint);
        ts2.timeToFinish = ts1.timeToFinish;
        ts2.spawnPosition = ts1.spawnPosition;
        switch(ts2.nodeType){
            case(1):
                ts2.pathPositions = ts1.pathPositions;
                ts2.timeLine = ts1.timeLine;
                break;
            case(2):
                ts2.startAngle = ts1.startAngle;
                break;
            case(3):
                ts2.timeHold = ts1.timeHold;
                break;
            default:
                break;
        }
        return ts2;
    }
    

}
