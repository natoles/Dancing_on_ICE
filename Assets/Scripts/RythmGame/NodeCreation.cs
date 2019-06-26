using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreation
{
    public enum Joint {Hand, RightHand, LeftHand}
    GameObject nodePrefab; 
    GameObject nodePrefabBH, nodePrefabBRH, nodePrefabBLH;
    GameObject nodePrefabLRH, nodePrefabLLH;
    GameObject nodePrefabAH, nodePrefabARH, nodePrefabALH;
    GameObject nodePrefabHRH, nodePrefabHLH;
    GameObject nodeInstance;
    GameObject main;

    public NodeCreation(){
        nodePrefabBH = Resources.Load("Prefabs/Nodes/BasicNodes/Prefab_BasicNode_Hand") as GameObject;
        nodePrefabBRH = Resources.Load("Prefabs/Nodes/BasicNodes/Prefab_BasicNode_RightHand") as GameObject;
        nodePrefabBLH = Resources.Load("Prefabs/Nodes/BasicNodes/Prefab_BasicNode_LeftHand") as GameObject;
        nodePrefabLRH = Resources.Load("Prefabs/Nodes/LineNodes/Prefab_LineNode_RightHand") as GameObject;
        nodePrefabLLH = Resources.Load("Prefabs/Nodes/LineNodes/Prefab_LineNode_LeftHand") as GameObject;
        nodePrefabAH = Resources.Load("Prefabs/Nodes/AngleNodes/Prefab_AngleNode_Hand") as GameObject;
        nodePrefabARH = Resources.Load("Prefabs/Nodes/AngleNodes/Prefab_AngleNode_RightHand") as GameObject;
        nodePrefabALH = Resources.Load("Prefabs/Nodes/AngleNodes/Prefab_AngleNode_LeftHand") as GameObject;
        nodePrefabHRH = Resources.Load("Prefabs/Nodes/HoldNodes/Prefab_HoldNode_RightHand") as GameObject;
        nodePrefabHLH = Resources.Load("Prefabs/Nodes/HoldNodes/Prefab_HoldNode_LeftHand") as GameObject;
        main = GameObject.Find("Main");
    }

    #region BasicNode
    //Creates a BasicNode for the body part joint, the node lasts timeToFinish seconds, at position spawnPosition  
    public void CreateBasicNode(Joint joint, float timeToFinish, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.Hand :
                nodeInstance = Object.Instantiate(nodePrefabBH, spawnPositon, Quaternion.Euler(0,0,0)); 
                break;
            case Joint.RightHand : 
                nodeInstance = Object.Instantiate(nodePrefabBRH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            case Joint.LeftHand : 
                nodeInstance = Object.Instantiate(nodePrefabBLH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
        BasicNode obj = nodeInstance.GetComponent<BasicNode>();
        obj.timeToFinish = timeToFinish;
        obj.transform.parent = main.transform;
    }
    #endregion

    #region LineNode
    //Creates a LineNode for the body part joint, the node lasts timeToFinish seconds,
    //travels for timeLine seconds, from spawnPosition to pos1 to pos2
    public void CreateLineNode(Joint joint, float timeToFinish, float timeLine, Vector3 spawnPosition, Vector3[] pathPositions){
        switch (joint)
        {
            case Joint.RightHand :
                nodeInstance = Object.Instantiate(nodePrefabLRH, spawnPosition, Quaternion.Euler(0,0,0));
                break;
            case Joint.LeftHand : 
                nodeInstance = Object.Instantiate(nodePrefabLLH, spawnPosition, Quaternion.Euler(0,0,0));
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
        LineNode obj = nodeInstance.GetComponent<LineNode>();
        obj.timeToFinish = timeToFinish;
        obj.timeLine = timeLine;
        obj.pathPositions = pathPositions;
        obj.transform.parent = main.transform;
    }
    #endregion

    #region CreateAngleNode
    //Creates a LineNode for the body part joint, the node lasts timeToFinish seconds, 
    //with an angle of startAngle, at position spawnPosition 
    public void CreateAngleNode(Joint joint, float timeToFinish, float startAngle, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.Hand : 
                nodeInstance = Object.Instantiate(nodePrefabAH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            case Joint.RightHand : 
                nodeInstance = Object.Instantiate(nodePrefabARH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            case Joint.LeftHand : 
                nodeInstance = Object.Instantiate(nodePrefabALH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
        AngleNode obj = nodeInstance.GetComponent<AngleNode>();
        obj.timeToFinish = timeToFinish;
        obj.startAngle = startAngle;
        obj.transform.parent = main.transform;
    }
    #endregion 

    #region HoldNode
    //Creates a BasicNode for the body part joint, the node lasts timeToFinish seconds, at position spawnPosition  
    public void CreateHoldNode(Joint joint, float timeToFinish, float timeHold, Vector3 spawnPositon){    
        switch (joint)
        {
            case Joint.RightHand : 
                nodeInstance = Object.Instantiate(nodePrefabHRH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            case Joint.LeftHand : 
                nodeInstance = Object.Instantiate(nodePrefabHLH, spawnPositon, Quaternion.Euler(0,0,0));
                break;
            default :
                Debug.Log("Pas normal");
                break;
        }
        HoldNode obj = nodeInstance.GetComponent<HoldNode>();
        obj.timeToFinish = timeToFinish;
        obj.timeHold = timeHold;
        obj.transform.parent = main.transform;
    }
    #endregion
}
