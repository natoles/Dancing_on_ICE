using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart; //Outer circle
    GameObject goal; //Inner circle
    Vector3 size; //Size of the inner circle 
    protected bool inCircle = false; //True if the correct joint is in the node
    protected float timeIn = 0; //Time since the joint has entered the node
    float timeFrame; //Time frame to make a PERFECT
    float timeToFinish = 3f; ///Time the node will take to destroy itself
    private IEnumerator _growth; 
    bool finished = false; //True if the node is finished
    public GameObject textMissed;
    GameObject main;
    
    void Start()
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
