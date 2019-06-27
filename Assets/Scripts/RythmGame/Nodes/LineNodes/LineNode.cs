using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LineNode : Node
{
    LineRenderer line; //The line the nde will follow
    public float timeLine; //Time the node will take to make his journey accross the screen. Farewell little node.
    IEnumerator moveLine; 
    bool moving = false; //Is the node moving along the lineRenderer ?
    bool finishedMoving = false; //Has the node finished his journey ?
    protected string joint; //Choice of the joint who will activate the node (Hand or Foot)
    float timeInside; //the time the player has to stay inside the node to get a certain score
    bool inCircle = false; //Is the player in the circle ?
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
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
            ChangeText(mtext.GetComponent<Text>(), "PERFECT", 30, Color.yellow, scorePerfect);  

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
            GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);       
            if (Time.time - timeInside >= timeLine * 2/3.0f){
                ChangeText(mtext.GetComponent<Text>(), "GREAT", 0, Color.magenta, scoreGreat);  
            }
            else {
                if (Time.time - timeInside >= timeLine * 1/3.0f)
                    ChangeText(mtext.GetComponent<Text>(), "BAD", -20, Color.blue, scoreBad);
                else ChangeText(mtext.GetComponent<Text>(), "MISSED", -50, Color.red, scoreMissed);
            }
            Destroy(gameObject);
        }
        
    }

    private IEnumerator MoveLine(){
        timeInside = Time.time;
        moving = true;
        float progress;
        float tmpTime;
        float fraction;
        float dist;

        for(int i = 0; i < pathPositions.Length; i++){
            tmpTime = 0;
            if (timePaths[i] > Time.deltaTime){
                progress = 0f;
                while(progress <= timePaths[i]){
                    if (i != 0)
                        transform.position = Vector3.Lerp(pathPositions[i - 1], pathPositions[i], progress/timePaths[i]);
                    else 
                        transform.position = Vector3.Lerp(line.GetPosition(0), pathPositions[i], progress/timePaths[i]);
                    progress += Time.deltaTime;
                    yield return null;
                }
            } else {
                tmpTime = timePaths[i++];
                while(tmpTime < Time.deltaTime) {
                    if (i == pathPositions.Length-1){
                        transform.position = pathPositions[i];
                        finishedMoving = true;
                        moving = false;
                        yield break;
                    }   
                    tmpTime += timePaths[i];
                    i += 1;
                }
                i -= 1;
                tmpTime = timePaths[i] - (tmpTime - Time.deltaTime);
                fraction = tmpTime/timePaths[i];
                transform.position = new Vector3((pathPositions[i].x - pathPositions[i-1].x) * fraction + pathPositions[i-1].x , 
                                                 (pathPositions[i].y - pathPositions[i-1].y) * fraction + pathPositions[i-1].y,
                                                 0);
                pathPositions[i-1] = transform.position;
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
