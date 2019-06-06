using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart; //Inner circle
    GameObject goal; //Outer circle
    Vector3 size; //Size of the outer circle 
    bool inCircle = false; //True if the correct joint is in the node
    float timeIn = 0; //Time since the joint has entered the node
    float timeFrame; //Time frame to make a PERFECT
    float timeToFinish = 3f; ///Time the node will take to destroy itself
    string member = "Hand"; //Choice of the joint who will activate the node (Hand or Foot)
    private IEnumerator _growth; 
    bool finished = false; //True if the node is finished
    
    void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        size = goal.transform.localScale;
        timeFrame = timeToFinish/10;

        //Graphical adjustment
        size[0] -= size[0]/12;
        size[1] -= size[1]/12;

        _growth = Growth(3f);
        StartCoroutine(_growth);
    }

    
    void Update()
    {
        //Scroe according to timing
        if (finished){
            if (inCircle)
                {
                    if (Time.time - timeIn <= timeFrame)
                        Debug.Log("PERFECT");
                    else 
                        Debug.Log("GOOD");
                } else {
                    Debug.Log("MISSED");
                }
            Destroy(gameObject);
        } 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == member){
            inCircle = true;
            timeIn = Time.time;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == member){
            inCircle = false;
        } 
    }

    //Interpolate the inner circle
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
