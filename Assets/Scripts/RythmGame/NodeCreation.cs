using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreation : MonoBehaviour
{
    public enum Joint {Hand, RightHand, LeftHand}
    GameObject nodePrefab; 
    TimeStamp ts = new TimeStamp(0,0,0); //To get default values

    GameObject nodePrefabBH, nodePrefabBRH, nodePrefabBLH;
    GameObject nodePrefabLRH, nodePrefabLLH;
    GameObject nodePrefabAH, nodePrefabARH, nodePrefabALH;
    Collider zoneH, zoneRH, zoneLH;
    GameObject nodeInstance;

    void Start(){
        nodePrefabBH = Resources.Load("Prefabs/Nodes/BasicNodes/Prefab_BasicNode_Hand") as GameObject;
        nodePrefabBRH = Resources.Load("Prefabs/Nodes/BasicNodes/Prefab_BasicNode_RightHand") as GameObject;
        nodePrefabBLH = Resources.Load("Prefabs/Nodes/BasicNodes/Prefab_BasicNode_LeftHand") as GameObject;
        nodePrefabLRH = Resources.Load("Prefabs/Nodes/LineNodes/Prefab_LineNode_RightHand") as GameObject;
        nodePrefabLLH = Resources.Load("Prefabs/Nodes/LineNodes/Prefab_LineNode_LeftHand") as GameObject;
        nodePrefabAH = Resources.Load("Prefabs/Nodes/AngleNodes/Prefab_AngleNode_Hand") as GameObject;
        nodePrefabARH = Resources.Load("Prefabs/Nodes/AngleNodes/Prefab_AngleNode_RightHand") as GameObject;
        nodePrefabALH = Resources.Load("Prefabs/Nodes/AngleNodes/Prefab_AngleNode_LeftHand") as GameObject;

        zoneH = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
        zoneRH = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
        zoneLH = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }

    #region BasicNode
    //Creates a BasicNode for the body part joint, the node lasts timeToFinish seconds, at position spawnPosition  
    public void CreateBasicNode(Joint joint, float timeToFinish, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.Hand : ;
                nodeInstance = Instantiate(nodePrefabBH, spawnPositon, Quaternion.Euler(0,0,0));
                nodeInstance.GetComponent<BasicNode_Hand>().timeToFinish = timeToFinish;
                break;
            case Joint.RightHand : 
                nodeInstance = GameObject.Instantiate(nodePrefabBRH, spawnPositon, Quaternion.Euler(0,0,0));
                nodeInstance.GetComponent<BasicNode_RightHand>().timeToFinish = timeToFinish;
                break;
            case Joint.LeftHand : 
                nodeInstance = Instantiate(nodePrefabBLH, spawnPositon, Quaternion.Euler(0,0,0));
                nodeInstance.GetComponent<BasicNode_LeftHand>().timeToFinish = timeToFinish;
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
    }

    public void CreateBasicNode(Joint joint, float timeToFinish){ 
        Vector3 randomSpawnPosition;
        switch (joint)
        {
            case Joint.Hand :
                randomSpawnPosition = RandomPointInBounds(zoneH.bounds);
                break;
            case Joint.RightHand : 
                randomSpawnPosition = RandomPointInBounds(zoneRH.bounds);
                break;
            case Joint.LeftHand : 
                randomSpawnPosition = RandomPointInBounds(zoneLH.bounds);
                break;
            default :
                randomSpawnPosition = RandomPointInBounds(zoneH.bounds);
                break;
        }
        CreateBasicNode(joint, timeToFinish, randomSpawnPosition);
    }

    public void CreateBasicNode(Joint joint){
        CreateBasicNode(joint, ts.defaultTimeToFinish);
    }
    #endregion

    #region LineNode
    //Creates a LineNode for the body part joint, the node lasts timeToFinish seconds,
    //travels for timeLine seconds, from spawnPosition to pos1 to pos2
    public void CreateLineNode(Joint joint, float timeToFinish, float timeLine, Vector3 spawnPosition, Vector3 pos1, Vector3 pos2){
        switch (joint)
        {
            case Joint.RightHand :
                nodeInstance = Instantiate(nodePrefabLRH, spawnPosition, Quaternion.Euler(0,0,0));
                LineNode_RightHand objR = nodeInstance.GetComponent<LineNode_RightHand>();
                objR.timeToFinish = timeToFinish;
                objR.timeLine = timeLine;
                objR.pos1 = pos1;
                objR.pos2 = pos2;
                break;
            case Joint.LeftHand : 
                nodeInstance = Instantiate(nodePrefabLLH, spawnPosition, Quaternion.Euler(0,0,0));
                LineNode_LeftHand objL = nodeInstance.GetComponent<LineNode_LeftHand>();
                objL.timeToFinish = timeToFinish;
                objL.timeLine = timeLine;
                objL.pos1 = pos1;
                objL.pos2 = pos2;
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
    }
    
    public void CreateLineNode(Joint joint, float timeToFinish, float timeLine){
        Vector3 randomSpawnPosition;
        switch (joint)
        {
            case Joint.RightHand : 
                randomSpawnPosition = RandomPointInBounds(zoneRH.bounds);
                break;
            case Joint.LeftHand : 
                randomSpawnPosition = RandomPointInBounds(zoneLH.bounds);
                break;
            default :
                randomSpawnPosition = RandomPointInBounds(zoneRH.bounds);
                break;
        }
        CreateLineNode(joint, timeToFinish, timeLine, randomSpawnPosition, Vector3.zero, Vector3.zero);
        
    }
    
    public void CreateLineNode(Joint joint){
        CreateLineNode(joint, ts.defaultTimeToFinish, ts.defaultTimeLine);
    }
    #endregion

    #region CreateAngleNode
    //Creates a LineNode for the body part joint, the node lasts timeToFinish seconds, 
    //with an angle of startAngle, at position spawnPosition 
    public void CreateAngleNode(Joint joint, float timeToFinish, float startAngle, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.Hand : 
                nodeInstance = Instantiate(nodePrefabAH, spawnPositon, Quaternion.Euler(0,0,0));
                nodeInstance.GetComponent<AngleNode_Hand>().timeToFinish = timeToFinish;
                nodeInstance.GetComponent<AngleNode_Hand>().startAngle = startAngle;
                break;
            case Joint.RightHand : 
                nodeInstance = Instantiate(nodePrefabARH, spawnPositon, Quaternion.Euler(0,0,0));
                nodeInstance.GetComponent<AngleNode_RightHand>().timeToFinish = timeToFinish;
                nodeInstance.GetComponent<AngleNode_RightHand>().startAngle = startAngle;
                break;
            case Joint.LeftHand : 
                nodeInstance = Instantiate(nodePrefabALH, spawnPositon, Quaternion.Euler(0,0,0));
                nodeInstance.GetComponent<AngleNode_LeftHand>().timeToFinish = timeToFinish;
                nodeInstance.GetComponent<AngleNode_LeftHand>().startAngle = startAngle;
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
    }

    public void CreateAngleNode(Joint joint, float timeToFinish){ 
        Vector3 randomSpawnPosition;
        switch (joint)
        {
            case Joint.Hand :
                randomSpawnPosition = RandomPointInBounds(zoneH.bounds);
                break;
            case Joint.RightHand : 
                randomSpawnPosition = RandomPointInBounds(zoneRH.bounds);
                break;
            case Joint.LeftHand : 
                randomSpawnPosition = RandomPointInBounds(zoneLH.bounds);
                break;
            default :
                randomSpawnPosition = RandomPointInBounds(zoneH.bounds);
                break;
        }
        float randomStartAngle = Random.Range(0,360);
        CreateAngleNode(joint, timeToFinish, randomStartAngle, randomSpawnPosition);
    }

    public void CreateAngleNode(Joint joint){
        CreateAngleNode(joint, ts.defaultTimeToFinish);
    }
    #endregion 
}
