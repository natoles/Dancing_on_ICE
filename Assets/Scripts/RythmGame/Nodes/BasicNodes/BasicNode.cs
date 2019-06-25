﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNode : Node
{

    
    void Update()
    {
        //Score according to timing
        if (finished){
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
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

}
