using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode_Hand : BasicNode
{
    string joint1 = "RightHand"; //Choice of the joint who will activate the node (Hand or Foot)
    string joint2 = "LeftHand"; //Choice of the joint who will activate the node (Hand or Foot)

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint1 || col.gameObject.tag == joint2){
            inCircle = true;
            timeIn = Time.time;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == joint1 || col.gameObject.tag == joint2){
            inCircle = false;
        } 
    }
}
