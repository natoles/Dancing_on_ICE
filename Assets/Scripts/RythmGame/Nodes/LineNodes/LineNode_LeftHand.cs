using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LineNode_LeftHand : LineNode
{
    
    public override void SetJoint(){
        spawnZone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
        joint = "LeftHand";
    }
    
}
