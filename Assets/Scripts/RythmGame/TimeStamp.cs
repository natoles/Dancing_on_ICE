using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStamp 
{
    public float timeSpawn;
    public int nodeType;
    public int joint;
    public float timeToFinish;
    public float timeLine;
    public Vector3 spawnPosition;
    public Vector3 pos1;
    public Vector3 pos2;
    public float time;
    public NodeCreation creator;

    //BasicNode Constructor
    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, Vector3 spawnPosition1){
        nodeType = nodeType1;
        joint = joint1;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        spawnPosition = spawnPosition1;
    }  

    public TimeStamp(float timeSpawn1, int nodeType1, int joint1, float timeToFinish1, float timeLine1, Vector3 spawnPosition1, Vector3 pos11, Vector3 pos21){
        nodeType = nodeType1;
        joint = joint1;
        timeLine = timeLine1;
        pos1 = pos11;
        pos2 = pos21;
        timeSpawn = timeSpawn1;
        timeToFinish = timeToFinish1;
        spawnPosition = spawnPosition1;
    }  
}
