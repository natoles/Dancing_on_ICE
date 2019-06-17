using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LineNode : Node
{
    LineRenderer line; //The line the nde will follow
    protected Collider spawnZone; //Th Zone in which the lineRenderer will be created
    public float timeLine = 5f; //Time the node will take to make his journey accross the screen. Farewell little node.
    float timeLine1; //Time to go from initial position to point 1
    float timeLine2; //Time to go from point 1 to point 2
    private IEnumerator moveLine; 
    bool moving = false; //Is the node moving along the lineRenderer ?
    bool finishedMoving = false; //Has the node finished his journey ?
    protected string joint; //Choice of the joint who will activate the node (Hand or Foot)
    float timeInside; //the time the player has to stay inside the node to get a certain score
    protected bool inCircle = false; //Is the player in the circle ?
    public Vector3 pos1;
    public Vector3 pos2;
    bool init = true;
    void Awake(){
        SetJoint();
    }
    public override void Start()
    {
        base.Start();  
        //spawnZone = GameObject.Find("SpawnZones/RH_zone").GetComponent<BoxCollider>();
        line = GetComponent<LineRenderer>();
        if (pos1 == Vector3.zero && pos2 == Vector3.zero)
        {
            pos1 = RandomPointInBounds(spawnZone.bounds);
            pos2 = RandomPointInBounds(spawnZone.bounds);
        }
          
    }
    void Update()
    {
        //init in Update so I can change the values of some variables
        if (init){
            init = false;

            //creation of the path
            line.SetPosition(0, transform.position);
            line.SetPosition(1, pos1);
            line.SetPosition(2, pos2);
            line.loop = false;

            //Time calculation
            float dist1 = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
            float dist2 = Vector3.Distance(line.GetPosition(1), line.GetPosition(2));
            float totalDistance =  dist1 + dist2;
            timeLine1 = dist1/totalDistance * timeLine;
            timeLine2 = dist2/totalDistance * timeLine;
        }
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
        float progress = 0;

        //First part
        while(progress <= timeLine1){
            Debug.Log(timeLine);
            Debug.Log(progress);
            Debug.Log(timeLine1);
            transform.position = Vector3.Lerp(line.GetPosition(0), line.GetPosition(1), progress/timeLine1);
            progress += Time.deltaTime;
            yield return null;
        }
        progress = 0f;

        //Second part
        while(progress <= timeLine2){
            transform.position = Vector3.Lerp(line.GetPosition(1), line.GetPosition(2), progress/timeLine2);
            progress += Time.deltaTime;
            yield return null;
        }
        finishedMoving = true;
        moving = false;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
    return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        0
    );
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
        Debug.Log("parent");
    }

}
