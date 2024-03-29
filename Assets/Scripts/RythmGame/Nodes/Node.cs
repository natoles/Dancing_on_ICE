﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    protected GameObject movingPart; //Outer circle
    protected GameObject goal; //Inner circle
    protected Vector3 size; //Size of the inner circle 
    public float timeToFinish; ///Time the node will take to destroy itself
    private IEnumerator _growth; 
    protected bool finished = false; //True if the node is finished
    protected bool active = true; //Is the collision detection active ?
    public bool destroyOnTouch;
    static int layerOrder = 0;
    
    protected GameObject main;
    protected float progress = 0; //progress of the node
    protected string joint;
    protected bool jointIn = false; //Did the player miss the node ?

    protected GameObject UI;
    string UIname = "UI";
    protected Scoring scoring;
    public GameObject textMissed; //Score text : MISSED, BAD, GREAT, PERFECT
    protected float timeFrame; //Time frame to make a PERFECT
    protected int scoreMissed = 0;
    protected int scoreBad = 5621;
    protected int scoreGreat = 8345;
    protected int scorePerfect = 15787;

    protected int frameCpt = 0;
    
    public virtual void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        size = goal.transform.localScale;
        timeFrame = timeToFinish/8;
        
        //Graphical adjustment
        size[0] += size[0]/8;
        size[1] += size[1]/8;

        //The most ancient node appers on top
        layerOrder--;
        //movingPart.GetComponent<SpriteRenderer>().sortingOrder = layerOrder; //Not sure about that
        goal.GetComponent<SpriteRenderer>().sortingOrder = layerOrder-1;

        _growth = Growth(timeToFinish);
        StartCoroutine(_growth);
        main = GameObject.Find("Main");
        UI = GameObject.Find(UIname);
        scoring = main.GetComponent<Scoring>();
        SetJoint();
    }

    public virtual void Update(){ 
        //To prevent accidental node activations(if the node spawns on the player's hands)
        frameCpt++;
        if(destroyOnTouch && frameCpt == 1 && jointIn){
            Debug.Log("enter coroutine");
            StartCoroutine(deactivateAtStart());
        }
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
        finished = true;
    }

    IEnumerator deactivateAtStart(){
        var col = GetComponent<CircleCollider2D>();
        active = false;
        yield return new WaitForSeconds(0.7f);
        active = true;
    }

    protected void ChangeText(Text theText, string displayed, int font, Color color, int score){
        theText.text = displayed;
        theText.fontSize += font;
        theText.color = color;
        scoring.AddScore(score); 
    }

    public virtual void SetJoint(){
        Debug.Log("abstract function");
    }
    
}
