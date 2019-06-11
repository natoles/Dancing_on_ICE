using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LineNode_Hand : Node
{

    LineRenderer line;
    Vector3 basePosition;
    Collider LH_zone; 
    float timeLine = 5f;
    float timeLine1;
    float timeLine2;
    private IEnumerator moveLine; 
    bool moving = false;
    bool finishedMoving = false;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        LH_zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, RandomPointInBounds(LH_zone.bounds));
        line.SetPosition(2, RandomPointInBounds(LH_zone.bounds));
        float dist1 = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
        float dist2 = Vector3.Distance(line.GetPosition(1), line.GetPosition(2));
        float totalDistance =  dist1 + dist2;
        timeLine1 = dist1/totalDistance * timeLine;
        timeLine2 = dist2/totalDistance * timeLine;
    }
    void Update()
    {
        if (finishedMoving){
            Destroy(gameObject);
        }
        if (!moving && finished){
            moveLine = MoveLine();
            StartCoroutine(moveLine);
        }
        
    }

    private IEnumerator MoveLine(){
        moving = true;
        float progress = 0;
        while(progress <= timeLine1){
            transform.position = Vector3.Lerp(line.GetPosition(0), line.GetPosition(1), progress/timeLine1);
            progress += Time.deltaTime;
            yield return null;
        }
        progress = 0f;
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
}
