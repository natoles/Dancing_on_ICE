using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LineNode : Node
{
    LineRenderer line; //The line the nde will follow
    public float timeLine = 5f; //Time the node will take to make his journey accross the screen. Farewell little node.
    private IEnumerator moveLine; 
    bool moving = false; //Is the node moving along the lineRenderer ?
    bool finishedMoving = false; //Has the node finished his journey ?
    protected string joint; //Choice of the joint who will activate the node (Hand or Foot)
    float timeInside; //the time the player has to stay inside the node to get a certain score
    protected bool inCircle = false; //Is the player in the circle ?
    public Vector3[] pathPositions;
    public float[] timePaths;

    public override void Start()
    {
        base.Start(); 
        SetJoint(); 
        line = GetComponent<LineRenderer>();  
        timePaths = new float[pathPositions.Length]; 

        //creation of the path
        line.positionCount = pathPositions.Length + 1;
        float dist;
        float totalDistance = 0;
        line.SetPosition(0, transform.position);
        for(int i = 0; i< pathPositions.Length; i++){
            line.SetPosition(i+1, pathPositions[i]);
            dist = Vector3.Distance(line.GetPosition(i), line.GetPosition(i+1));
            totalDistance += dist; 
        }
        

        //Time calculation
        for(int i = 0; i< pathPositions.Length; i++){
            dist = Vector3.Distance(line.GetPosition(i), line.GetPosition(i+1));
            timePaths[i] = dist/totalDistance * timeLine;
        }
 
    }
    void Update()
    {
        line.loop = false;
        movingPart.transform.Rotate(0f,0f,1f);

        //If player completed the entire path
        if (finishedMoving){
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            mtext.GetComponent<Text>().text = "PERFECT";
            mtext.GetComponent<Text>().fontSize += 30;
            mtext.GetComponent<Text>().color = Color.yellow;
            main.GetComponent<Scoring>().Score += 15369;
            Debug.Log("PERFECT");

            //reset variable to avoid entering in other loop after destruction
            moving = false; 
            finished = false;
            Destroy(gameObject);
        }

        //Start the journey
        if (!moving && finished){
            moveLine = MoveLine();
            StartCoroutine(moveLine);
        }

        //If the player fail to follow the entire path
        if(moving && !inCircle)
        {
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            if (Time.time - timeInside >= timeLine * 2/3.0f){
                mtext.GetComponent<Text>().text = "GREAT";
                mtext.GetComponent<Text>().fontSize += 0;
                mtext.GetComponent<Text>().color = Color.magenta;
                main.GetComponent<Scoring>().Score += 8345;
                Debug.Log("GOOD");
            }
            else {
                if (Time.time - timeInside >= timeLine * 1/3.0f){
                    mtext.GetComponent<Text>().text = "BAD";
                    mtext.GetComponent<Text>().fontSize -= 20;
                    mtext.GetComponent<Text>().color = Color.blue;
                    main.GetComponent<Scoring>().Score += 5621;
                    Debug.Log("BAD");
                }
                else 
                {
                    mtext.GetComponent<Text>().text = "MISSED";
                    mtext.GetComponent<Text>().fontSize -= 50;
                    mtext.GetComponent<Text>().color = Color.red;
                    main.GetComponent<Scoring>().Score += 0; //CHANGE TO 0
                    Debug.Log("MISSED");
                }
            }
            Destroy(gameObject);
        }
        
    }

    private IEnumerator MoveLine(){
        timeInside = Time.time;
        moving = true;
        float progress;

        for(int i = 0; i< pathPositions.Length; i++){
            progress = 0f;
            while(progress <= timePaths[i]){
                transform.position = Vector3.Lerp(line.GetPosition(i), line.GetPosition(i+1), progress/timePaths[i]);
                progress += Time.deltaTime;
                yield return null;
            }
        }
        finishedMoving = true;
        moving = false;
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

    public virtual void SetJoint(){
        Debug.Log("abstract function");
    }

}
