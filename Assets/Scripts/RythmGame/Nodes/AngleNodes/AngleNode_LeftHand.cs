using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleNode_LeftHand : AngleNode
{
    string joint = "LeftHand"; //Choice of the joint who will activate the node (Hand or Foot)

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            finished = true;
        } 
    }
}
