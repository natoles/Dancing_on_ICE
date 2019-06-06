using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart; //Inner circle
    GameObject goal; //Outer circle
    Vector3 size; //Size of the outer circle 
    protected bool inCircle = false; //True if the correct joint is in the node
    protected float timeIn = 0; //Time since the joint has entered the node
    float timeFrame; //Time frame to make a PERFECT
    float timeToFinish = 3f; ///Time the node will take to destroy itself
    private IEnumerator _growth; 
    bool finished = false; //True if the node is finished
    public GameObject textMissed;
    
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
        
        //Score according to timing
        if (finished){
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            if (inCircle)
                {              
                    if (Time.time - timeIn <= timeFrame){
                        mtext.GetComponent<Text>().text = "PERFECT";
                        mtext.GetComponent<Text>().fontSize += 30;
                        mtext.GetComponent<Text>().color = Color.yellow;
                        Debug.Log("PERFECT");
                    }
                    else {
                        if (Time.time - timeIn <= timeFrame * 3){
                            mtext.GetComponent<Text>().text = "GREAT";
                            mtext.GetComponent<Text>().fontSize += 0;
                            mtext.GetComponent<Text>().color = Color.magenta;
                            Debug.Log("GOOD");
                        }
                        else {
                            mtext.GetComponent<Text>().text = "BAD";
                            mtext.GetComponent<Text>().fontSize -= 20;
                            mtext.GetComponent<Text>().color = Color.blue;
                            Debug.Log("BAD");
                        }
                    }
                } else {
                    ShowText();
                    mtext.GetComponent<Text>().text = "MISSED";
                    mtext.GetComponent<Text>().fontSize -= 50;
                    mtext.GetComponent<Text>().color = Color.gray;
                    Debug.Log("MISSED");
                }
            Destroy(gameObject);
        } 
    }

    void ShowText(){
        
        
    }

    //Interpolates the inner circle
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
