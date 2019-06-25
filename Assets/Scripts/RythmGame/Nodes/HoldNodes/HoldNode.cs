using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNode : Node
{
    protected bool inCircle = false;
    float timeToHold = 3f;
    protected string joint = "Default";
    int scoreSecond = 15;
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            inCircle = true;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            inCircle = false;
        } 
    }
/* 
    private IEnumerator Hold(){
        while (timeToHold > 0){
            timeToHold -= Time.deltaTime;
            if (inCircle){
                scoring.AddScore(15);
            }
        }
    }*/
}
