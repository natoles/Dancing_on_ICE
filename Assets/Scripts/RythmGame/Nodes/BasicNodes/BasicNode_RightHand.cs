using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode_RightHand : BasicNode
{
    string joint = "RightHand"; //Choice of the joint who will activate the node (Hand or Foot)

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            finished = true;
        } 
    }
}
