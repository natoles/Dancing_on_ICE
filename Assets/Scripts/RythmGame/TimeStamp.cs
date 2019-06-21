using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeStamp
{
    public float timeSpawn;
    public int nodeType;
    public int joint;
    public float timeToFinish;
    public float timeLine;
    public float startAngle;
    public Vector3 spawnPosition;
    public Vector3[] pathPositions;
    public float time;
    public Uri Uri;
    public int Count;
    public float defaultTimeToFinish = 3f;
    public float defaultTimeLine = 5f;

    //Simplified constructor(default values)
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
        timeToFinish = defaultTimeToFinish;
        timeLine = defaultTimeLine;
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

    //AngleNode Constructor 
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, float startAngle1, Vector3 spawnPosition1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        startAngle = startAngle1;
        spawnPosition = spawnPosition1;
    } 

    public TimeStamp DeepCopyTS(TimeStamp ts1){
        TimeStamp ts2 = new TimeStamp(ts1.timeSpawn, ts1.nodeType, ts1.joint);
        ts2.timeToFinish = ts1.timeToFinish;
        ts1.spawnPosition = ts2.spawnPosition;
        switch(ts2.nodeType){
            case(1):
                ts2.pathPositions = ts1.pathPositions;
                ts2.timeLine = ts1.timeLine;
                break;
            case(2):
                ts2.startAngle = ts1.startAngle;
                break;
            default:
                break;
        }
        return ts2;
    }
    

}
