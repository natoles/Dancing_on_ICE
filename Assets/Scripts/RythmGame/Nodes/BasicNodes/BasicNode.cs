using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNode : Node
{

    
    void Update()
    {
        //Score according to timing
        if (finished){
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            if (inCircle)
                {              
                    if (Time.time - timeIn <= timeFrame){
                        mtext.GetComponent<Text>().text = "PERFECT";
                        mtext.GetComponent<Text>().fontSize += 30;
                        mtext.GetComponent<Text>().color = Color.yellow;
                        main.GetComponent<Main>().Score += 15369;
                        Debug.Log("PERFECT");
                    }
                    else {
                        if (Time.time - timeIn <= timeFrame * 3){
                            mtext.GetComponent<Text>().text = "GREAT";
                            mtext.GetComponent<Text>().fontSize += 0;
                            mtext.GetComponent<Text>().color = Color.magenta;
                            main.GetComponent<Main>().Score += 8345;
                            Debug.Log("GOOD");
                        }
                        else {
                            mtext.GetComponent<Text>().text = "BAD";
                            mtext.GetComponent<Text>().fontSize -= 20;
                            mtext.GetComponent<Text>().color = Color.blue;
                            main.GetComponent<Main>().Score += 5621;
                            Debug.Log("BAD");
                        }
                    }
                } else {
                    mtext.GetComponent<Text>().text = "MISSED";
                    mtext.GetComponent<Text>().fontSize -= 50;
                    mtext.GetComponent<Text>().color = Color.gray;
                    main.GetComponent<Main>().Score += 15369; //CHANGE TO 0
                    Debug.Log("MISSED");
                }
            Destroy(gameObject);
        } 
    }

    
    

}
