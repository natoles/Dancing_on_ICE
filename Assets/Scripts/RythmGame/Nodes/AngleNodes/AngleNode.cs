using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleNode : Node
{
    GameObject mtext;

    public float startAngle;

    public override void Start(){
        base.Start();
        transform.Rotate(new Vector3(0,0,startAngle));
    }
    void Update()
    {
        if (movingPart.GetComponent<EnterLava>().touchedLava){
            mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
            ChangeText(mtext.GetComponent<Text>(), "BOOM!", 45, Color.black, scoreMissed);  
            Destroy(gameObject);
        }   

        if (destroyOnTouch){
            //Score according to timing
            if (finished){
                GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
                if (!jointIn){
                    ChangeText(mtext.GetComponent<Text>(), "MISSED", -50, Color.red, scoreMissed);
                } else {             
                    if (timeToFinish - progress <= timeFrame){
                        ChangeText(mtext.GetComponent<Text>(), "PERFECT", 30, Color.yellow, scorePerfect);  
                    }
                    else {
                        if (timeToFinish - progress <= timeFrame * 3){
                            ChangeText(mtext.GetComponent<Text>(), "GREAT", 0, Color.magenta, scoreGreat);  
                        }
                        else {
                            ChangeText(mtext.GetComponent<Text>(), "BAD", -20, Color.blue, scoreBad);
                        }
                    }
            }
            Destroy(gameObject);
            }
        } else {
            if (finished){
                GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
                if (!jointIn) ChangeText(mtext.GetComponent<Text>(), "MISSED", -50, Color.red, scoreMissed);      
                else ChangeText(mtext.GetComponent<Text>(), "PERFECT", 30, Color.yellow, scorePerfect);
                Destroy(gameObject);                         
            }
        } 
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            jointIn = true;
        } 

        if (destroyOnTouch) finished = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            jointIn = false;
        } 
    }
}
