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
    public Vector3 pos1;
    public Vector3 pos2;
    public float time;
    public Uri Uri;
    public int Count;
    public float defaultTimeToFinish = 3f;
    public float defaultTimeLine = 5f;
    public bool specifiedPosition;

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
        specifiedPosition = true;
    }  

    //LineNode Constructor
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, float timeLine1, Vector3 spawnPosition1, Vector3 pos11, Vector3 pos21){
        nodeType = nodeType1;
        joint = joint1;
        timeLine = timeLine1;
        pos1 = pos11;
        pos2 = pos21;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        spawnPosition = spawnPosition1;
        specifiedPosition = true;
    }  

    //AngleNode Constructor 
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, float startAngle1, Vector3 spawnPosition1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        startAngle = startAngle1;
        spawnPosition = spawnPosition1;
        specifiedPosition = true;
    } 

    public TimeStamp DeepCopyTS(TimeStamp ts1){
        TimeStamp ts2 = new TimeStamp(ts1.timeSpawn, ts1.nodeType, ts1.joint);
        ts2.timeToFinish = ts1.timeToFinish;
        ts2.specifiedPosition = true;
        ts1.spawnPosition = ts2.spawnPosition;
        switch(ts2.nodeType){
            case(1):
                ts2.pos1 = ts1.pos1;
                ts2.pos2 = ts1.pos2;
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
