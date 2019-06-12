using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreation : MonoBehaviour
{
    public enum Joint {Hand, RightHand, LeftHand}
    GameObject nodePrefab; 
    Collider zone;
    float defaultTimeToFinish = 3f;
    float defaultTimeLine = 5f;

    public static Vector3 RandomPointInBounds(Bounds bounds) {
    return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        0
    );
    }

    //Creates a BasicNode for the body part joint, the nose lasts timeToFinish seconds, at position spawnPosition  
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
        CreateBasicNode(joint, defaultTimeToFinish);
    }


    public void CreateLineNode(Joint joint, float timeToFinish, float timeLine, Vector3 spawnPosition){
        switch (joint)
        {
            case Joint.RightHand :
                nodePrefab = GameObject.Find("Prefab_LineNode_RightHand");
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                GameObject newLineNodeRightHand = Instantiate(nodePrefab, spawnPosition, Quaternion.Euler(0,0,0));
                newLineNodeRightHand.GetComponent<LineNode_RightHand>().enabled = true;
                newLineNodeRightHand.GetComponent<LineNode_RightHand>().timeToFinish = timeToFinish;
                newLineNodeRightHand.GetComponent<LineNode_RightHand>().timeLine = timeLine;
                break;
            case Joint.LeftHand : 
                nodePrefab = GameObject.Find("Prefab_LineNode_LeftHand");
                zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
                GameObject newLineNodeLeftHand = Instantiate(nodePrefab, spawnPosition, Quaternion.Euler(0,0,0));
                newLineNodeLeftHand.GetComponent<LineNode_LeftHand>().enabled = true;
                newLineNodeLeftHand.GetComponent<LineNode_LeftHand>().timeToFinish = timeToFinish;
                newLineNodeLeftHand.GetComponent<LineNode_LeftHand>().timeLine = timeLine;
                break;
            default :
                nodePrefab = GameObject.Find("Prefab_LineNode_RightHand");
                zone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
                GameObject newLineNodeDefault = Instantiate(nodePrefab, spawnPosition, Quaternion.Euler(0,0,0));
                newLineNodeDefault.GetComponent<LineNode_RightHand>().enabled = true;
                newLineNodeDefault.GetComponent<LineNode_RightHand>().timeToFinish = timeToFinish;
                newLineNodeDefault.GetComponent<LineNode_RightHand>().timeLine = timeLine;
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
        CreateLineNode(joint, timeToFinish, timeLine, randomSpawnPosition);
        
    }
    
    public void CreateLineNode(Joint joint){
        CreateLineNode(joint, defaultTimeToFinish, defaultTimeLine);
    }
}
