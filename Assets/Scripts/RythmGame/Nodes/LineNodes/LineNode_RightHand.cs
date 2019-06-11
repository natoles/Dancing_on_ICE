using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineNode_RightHand : LineNode
{
    // Start is called before the first frame update
    public override void SetJoint(){
        spawnZone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
        joint = "RightHand";
    }
}
