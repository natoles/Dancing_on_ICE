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
            ChangeText(mtext.GetComponent<Text>(), "BOOM!", 45, Color.black, 0);  
            Destroy(gameObject);
        }   

        //Score according to timing
        if (finished){
            mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
            if (missed){
                ChangeText(mtext.GetComponent<Text>(), "MISSED", -50, Color.red, 0);
            } else {             
                if (timeToFinish - progress <= timeFrame){
                    ChangeText(mtext.GetComponent<Text>(), "PERFECT", 30, Color.yellow, 15369);
                }
                else {
                    if (timeToFinish - progress <= timeFrame * 3){
                        ChangeText(mtext.GetComponent<Text>(), "GREAT", 0, Color.magenta, 8345);
                    }
                    else {
                        ChangeText(mtext.GetComponent<Text>(), "BAD", -20, Color.blue, 5621);
                    }
                }
            }
            Destroy(gameObject);
        } 
    }

    void ChangeText(Text theText, string displayed, int font, Color color, int score){
        theText.text = displayed;
        theText.fontSize += font;
        theText.color = color;
        main.GetComponent<Scoring>().Score += score; 
        Debug.Log(displayed);
    }

    
}
