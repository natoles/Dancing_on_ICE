using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    
    protected GameObject movingPart; //Outer circle
    GameObject goal; //Inner circle
    protected Vector3 size; //Size of the inner circle 
    protected float timeFrame; //Time frame to make a PERFECT
    public float timeToFinish; ///Time the node will take to destroy itself
    private IEnumerator _growth; 
    protected bool finished = false; //True if the node is finished
    public GameObject textMissed; //Score text : MISSED, BAD, GREAT, PERFECT
    protected GameObject main;
    protected float progress = 0; //progress of the node
    protected bool missed = false; //Did the player miss the node ?
    protected GameObject UI;
    string UIname = "UI";
    protected int scoreMissed = 0;
    protected int scoreBad = 5621;
    protected int scoreGreat = 8345;
    protected int scorePerfect = 15787;
    protected Scoring scoring;

    public virtual void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        size = goal.transform.localScale;
        timeFrame = timeToFinish/8;
        
        //Graphical adjustment
        size[0] += size[0]/13;
        size[1] += size[1]/13;

        _growth = Growth(timeToFinish);
        StartCoroutine(_growth);
        main = GameObject.Find("Main");
        UI = GameObject.Find(UIname);
        scoring = main.GetComponent<Scoring>();

    }


    //Interpolates the outer circle
    private IEnumerator Growth(float timeGrowth){
        progress = 0;
        Vector3 initialScale = movingPart.transform.localScale;
        Vector3 finalScale = size;
        while(progress <= timeGrowth){
            movingPart.transform.localScale = Vector3.Lerp(initialScale, finalScale, progress/timeGrowth);
            progress += Time.deltaTime;
            yield return null;
        }
        movingPart.transform.localScale = finalScale;
        missed = true;
        finished = true;
    }

    protected void ChangeText(Text theText, string displayed, int font, Color color, int score){
        theText.text = displayed;
        theText.fontSize += font;
        theText.color = color;
        scoring.AddScore(score); 
        //Debug.Log(displayed);
    }
    
}
