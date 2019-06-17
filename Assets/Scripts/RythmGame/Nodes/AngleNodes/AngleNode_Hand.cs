using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleNode_Hand : AngleNode
{
    string joint1 = "RightHand"; //Choice of the joint who will activate the node (Hand or Foot)
    string joint2 = "LeftHand"; //Choice of the joint who will activate the node (Hand or Foot)

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint1 || col.gameObject.tag == joint2){
            finished = true;
        } 
    }

}
