using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNode : Node
{
    public override void Update()
    {
        base.Update();
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

        if (destroyOnTouch && frameCpt > 1 && active && jointIn) finished = true; //To check if joint still in after reactivation   
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            jointIn = true;
        } 

        if (destroyOnTouch && frameCpt > 1 && active) finished = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            jointIn = false;
            active = true;
        } 
    }

}
