using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HoldNode : Node
{
    protected bool inCircle = false;
    public float timeHold;
    protected string joint = "Default";
    int holdScore = 15;
    bool hold = false; 
    bool holdFinished = false;
    bool showScore = false; //Shows the score every second

    IEnumerator holdIt;

    void Update(){
        if (finished && !hold){
            holdIt = Hold();
            StartCoroutine(holdIt);
            hold = true;
        }

        if (holdFinished) Destroy(gameObject);

        if (showScore){
            showScore = false;
            GameObject mtext = Instantiate(textMissed, this.transform.position + new Vector3(0,1f,0), Quaternion.identity, UI.transform);
            if (inCircle){
                ChangeText(mtext.GetComponent<Text>(), "GREAT", 0, Color.magenta, 0);  
            } else ChangeText(mtext.GetComponent<Text>(), "MISSED", 0, Color.red, scoreMissed);  
        }
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
 
    private IEnumerator Hold(){
        float timeLeft = timeHold;
        while (timeLeft > 0){
            timeLeft -= Time.deltaTime;
            movingPart.transform.Rotate(0f,0f,7f * (timeHold-timeLeft)/timeHold);
            if (inCircle) scoring.AddScore(holdScore);
            //Debug.Log((Math.Floor(timeLeft + Time.deltaTime) != Math.Floor(timeLeft)));
            if (Math.Floor(timeLeft + Time.deltaTime) != Math.Floor(timeLeft)) showScore = true;
            yield return null;
        }
        holdFinished = true;
    }

    
}
