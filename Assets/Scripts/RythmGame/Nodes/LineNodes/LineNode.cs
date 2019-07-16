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
    float timeInside; //the time the player has to stay inside the node to get a certain score
    public Vector3[] pathPositions;
    public float[] timePaths;
    Color baseColor;
    Color inColor;
    SpriteRenderer movingPartSprite;
    int scoreLine = 11;

    public override void Start()
    {
        base.Start(); 
        line = GetComponent<LineRenderer>();  
        timePaths = new float[pathPositions.Length]; 

        //Saves the base color
        movingPartSprite = movingPart.GetComponent<SpriteRenderer>();
        baseColor = movingPartSprite.color; 
        inColor = Color.black;

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

        //Changes the color if the joint touches the node 
        if (jointIn){
            movingPartSprite.color = new Color(1f, 0.95f, 0.3f, 1f) ;
        } else{
            movingPartSprite.color = baseColor;
        } 

        //If player completed the entire path
        if (finishedMoving){
            //GameObject mtext = Instantiate(textMissed, this.transform.position, Quaternion.identity, UI.transform);
            //ChangeText(mtext.GetComponent<Text>(), "PERFECT", 30, Color.yellow, scorePerfect);  

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

        if (moving && jointIn){
            scoring.AddScore(scoreLine);
        }
        
    }

    private IEnumerator MoveLine(){
        timeInside = Time.time;
        moving = true;
        float progress;
        float tmpTime;
        float fraction;

        for(int i = 0; i < pathPositions.Length; i++){
            tmpTime = 0;
            if (timePaths[i] > Time.deltaTime || i == 0 || i == pathPositions.Length-1){
                progress = 0f;
                while(progress <= timePaths[i]){
                    if (i != 0)
                        transform.position = Vector3.Lerp(pathPositions[i - 1], pathPositions[i], progress/timePaths[i]);
                    else 
                        transform.position = Vector3.Lerp(line.GetPosition(0), pathPositions[i], progress/timePaths[i]);
                    progress += Time.deltaTime;
                    yield return null;
                }
            } else {  //If the time to reach the other point is < to deltaTime
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
            jointIn = true;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == joint){
            jointIn = false;
        } 
    }

}
