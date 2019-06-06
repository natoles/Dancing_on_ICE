using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart;
    GameObject goal;
    float speed = 1f;
    float size;
    bool inCircle = false;
    float timeIn = 0;
    float timeFrame = 1f; //Time to make a PERFECT
    string member = "Hand";
    
    void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        size = goal.transform.lossyScale.x;
    }

    
    void Update()
    {
        movingPart.transform.localScale += new Vector3(speed/100, speed/100, 0);
        if (movingPart.transform.lossyScale.x >= size - size/12){
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
        //Debug.Log("Hey");
        if (col.gameObject.tag == member){
            inCircle = true;
            //Debug.Log("I'm in");
            timeIn = Time.time;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == member){
            inCircle = false;
        } 
    }

}
