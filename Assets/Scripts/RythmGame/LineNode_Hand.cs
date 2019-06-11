using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LineNode_Hand : Node
{

    LineRenderer line;
    Vector3 basePosition;
    Collider LH_zone; 
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        LH_zone = GameObject.Find("SpawnZones/LH_zone").GetComponent<BoxCollider>();
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, RandomPointInBounds(LH_zone.bounds));
        line.SetPosition(2, RandomPointInBounds(LH_zone.bounds));
    }
    void Update()
    {
        
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
    return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        0
    );
    }
}
