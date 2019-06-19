using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreation : MonoBehaviour
{
    public enum Joint {Hand, RightHand, LeftHand}
    GameObject nodePrefab; 
    Collider zone;
    TimeStamp ts = new TimeStamp(0,0,0); //To get default values

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }

    //Creates a BasicNode for the body part joint, the node lasts timeToFinish seconds, at position spawnPosition  
    public void CreateBasicNode(Joint joint, float timeToFinish, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.Hand : 
                nodePrefab = GameObject.Find("Prefab_BasicNode_Hand");
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                GameObject newBasicNodeHand = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newBasicNodeHand.GetComponent<BasicNode_Hand>().enabled = true;
                newBasicNodeHand.GetComponent<BasicNode_Hand>().timeToFinish = timeToFinish;
                break;
            case Joint.RightHand : 
                nodePrefab = GameObject.Find("Prefab_BasicNode_RightHand");
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                GameObject newBasicNodeRightHand = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newBasicNodeRightHand.GetComponent<BasicNode_RightHand>().enabled = true;
                newBasicNodeRightHand.GetComponent<BasicNode_RightHand>().timeToFinish = timeToFinish;
                break;
            case Joint.LeftHand : 
                nodePrefab = GameObject.Find("Prefab_BasicNode_LeftHand");
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                GameObject newBasicNodeLeftHand = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newBasicNodeLeftHand.GetComponent<BasicNode_LeftHand>().enabled = true;
                newBasicNodeLeftHand.GetComponent<BasicNode_LeftHand>().timeToFinish = timeToFinish;
                break;
            default :
                nodePrefab = GameObject.Find("Prefab_BasicNode_Hand");
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                GameObject newBasicNodeDefault = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newBasicNodeDefault.GetComponent<BasicNode_Hand>().enabled = true;
                newBasicNodeDefault.GetComponent<BasicNode_Hand>().timeToFinish = timeToFinish;
                break;
        }
    }

    public void CreateBasicNode(Joint joint, float timeToFinish){ 
        Vector3 randomSpawnPosition;
        switch (joint)
        {
            case Joint.Hand :
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>(); 
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            case Joint.RightHand : 
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            case Joint.LeftHand : 
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            default :
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
        }
        CreateBasicNode(joint, timeToFinish, randomSpawnPosition);
    }

    public void CreateBasicNode(Joint joint){
        CreateBasicNode(joint, ts.defaultTimeToFinish);
    }

    //Creates a LineNode for the body part joint, the node lasts timeToFinish seconds,
    //travels for timeLine seconds, from spawnPosition to pos1 to pos2
    public void CreateLineNode(Joint joint, float timeToFinish, float timeLine, Vector3 spawnPosition, Vector3 pos1, Vector3 pos2){
        switch (joint)
        {
            case Joint.RightHand :
                nodePrefab = GameObject.Find("Prefab_LineNode_RightHand");
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                GameObject newLineNodeRightHand = Instantiate(nodePrefab, spawnPosition, Quaternion.Euler(0,0,0));
                newLineNodeRightHand.GetComponent<LineRenderer>().enabled = true;
                LineNode_RightHand objR = newLineNodeRightHand.GetComponent<LineNode_RightHand>();
                objR.enabled = true;
                objR.timeToFinish = timeToFinish;
                objR.timeLine = timeLine;
                objR.pos1 = pos1;
                objR.pos2 = pos2;
                break;
            case Joint.LeftHand : 
                nodePrefab = GameObject.Find("Prefab_LineNode_LeftHand");
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                GameObject newLineNodeLeftHand = Instantiate(nodePrefab, spawnPosition, Quaternion.Euler(0,0,0));
                newLineNodeLeftHand.GetComponent<LineRenderer>().enabled = true;
                LineNode_LeftHand objL = newLineNodeLeftHand.GetComponent<LineNode_LeftHand>();
                objL.enabled = true;
                objL.timeToFinish = timeToFinish;
                objL.timeLine = timeLine;
                objL.pos1 = pos1;
                objL.pos2 = pos2;
                break;
            default :
                nodePrefab = GameObject.Find("Prefab_LineNode_RightHand");
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                GameObject newLineNodeDefault = Instantiate(nodePrefab, spawnPosition, Quaternion.Euler(0,0,0));
                newLineNodeDefault.GetComponent<LineRenderer>().enabled = true;
                LineNode_RightHand objD = newLineNodeDefault.GetComponent<LineNode_RightHand>();
                objD.enabled = true;
                objD.timeToFinish = timeToFinish;
                objD.timeLine = timeLine;
                objD.pos1 = pos1;
                objD.pos2 = pos2;
                break;
        }
    }
    
    public void CreateLineNode(Joint joint, float timeToFinish, float timeLine){
        Vector3 randomSpawnPosition;
        switch (joint)
        {
            case Joint.Hand :
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>(); 
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            case Joint.RightHand : 
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            case Joint.LeftHand : 
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            default :
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
        }
        CreateLineNode(joint, timeToFinish, timeLine, randomSpawnPosition, Vector3.zero, Vector3.zero);
        
    }
    
    public void CreateLineNode(Joint joint){
        CreateLineNode(joint, ts.defaultTimeToFinish, ts.defaultTimeLine);
    }

    public void CreateAngleNode(Joint joint, float timeToFinish, float startAngle, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.Hand : 
                nodePrefab = GameObject.Find("Prefab_AngleNode_Hand");
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                GameObject newAngleNodeHand = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newAngleNodeHand.GetComponent<AngleNode_Hand>().enabled = true;
                newAngleNodeHand.GetComponent<AngleNode_Hand>().timeToFinish = timeToFinish;
                newAngleNodeHand.GetComponent<AngleNode_Hand>().startAngle = startAngle;
                break;
            case Joint.RightHand : 
                nodePrefab = GameObject.Find("Prefab_AngleNode_RightHand");
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                GameObject newAngleNodeRightHand = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newAngleNodeRightHand.GetComponent<AngleNode_RightHand>().enabled = true;
                newAngleNodeRightHand.GetComponent<AngleNode_RightHand>().timeToFinish = timeToFinish;
                newAngleNodeRightHand.GetComponent<AngleNode_RightHand>().startAngle = startAngle;
                break;
            case Joint.LeftHand : 
                nodePrefab = GameObject.Find("Prefab_AngleNode_LeftHand");
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                GameObject newAngleNodeLeftHand = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newAngleNodeLeftHand.GetComponent<AngleNode_LeftHand>().enabled = true;
                newAngleNodeLeftHand.GetComponent<AngleNode_LeftHand>().timeToFinish = timeToFinish;
                newAngleNodeLeftHand.GetComponent<AngleNode_LeftHand>().startAngle = startAngle;
                break;
            default :
                nodePrefab = GameObject.Find("Prefab_AngleNode_Hand");
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                GameObject newAngleNodeDefault = Instantiate(nodePrefab, spawnPositon, Quaternion.Euler(0,0,0));
                newAngleNodeDefault.GetComponent<AngleNode_Hand>().enabled = true;
                newAngleNodeDefault.GetComponent<AngleNode_Hand>().timeToFinish = timeToFinish;
                newAngleNodeDefault.GetComponent<AngleNode_Hand>().startAngle = startAngle;
                break;
        }
    }

    public void CreateAngleNode(Joint joint, float timeToFinish){ 
        Vector3 randomSpawnPosition;
        switch (joint)
        {
            case Joint.Hand :
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>(); 
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            case Joint.RightHand : 
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            case Joint.LeftHand : 
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
            default :
                zone = GameObject.Find("SpawnZones/H_zone").GetComponent<BoxCollider>();
                randomSpawnPosition = RandomPointInBounds(zone.bounds);
                break;
        }
        float randomStartAngle = Random.Range(0,360);
        CreateAngleNode(joint, timeToFinish, randomStartAngle, randomSpawnPosition);
    }

    public void CreateAngleNode(Joint joint){
        CreateAngleNode(joint, ts.defaultTimeToFinish);
    }

}
