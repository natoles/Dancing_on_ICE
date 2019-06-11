using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    
    protected GameObject movingPart; //Outer circle
    GameObject goal; //Inner circle
    protected Vector3 size; //Size of the inner circle 
    protected bool inCircle = false; //True if the correct joint is in the node
    protected float timeIn = 0; //Time since the joint has entered the node
    protected float timeFrame; //Time frame to make a PERFECT
    float timeToFinish = 3f; ///Time the node will take to destroy itself
    private IEnumerator _growth; 
    protected bool finished = false; //True if the node is finished
    public GameObject textMissed; //Score text : MISSED, BAD, GREAT, PERFECT
    protected GameObject main;

    public virtual void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        size = goal.transform.localScale;
        timeFrame = timeToFinish/10;
        
        //Graphical adjustment
        size[0] += size[0]/12;
        size[1] += size[1]/12;

        _growth = Growth(timeToFinish);
        StartCoroutine(_growth);
        main = GameObject.Find("Main");
        
    }


    //Interpolates the outer circle
    private IEnumerator Growth(float timeGrowth){
        float progress = 0;
        Vector3 initialScale = movingPart.transform.localScale;
        Vector3 finalScale = size;
        while(progress <= timeGrowth){
            movingPart.transform.localScale = Vector3.Lerp(initialScale, finalScale, progress/timeGrowth);
            progress += Time.deltaTime;
            yield return null;
        }
        movingPart.transform.localScale = finalScale;
        finished = true;
    }
    
}
