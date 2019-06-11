using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_Node : MonoBehaviour
{
    LineRenderer lr;
    public GameObject objectTest;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, objectTest.transform.position);
        lr.SetPosition(1, new Vector3(10f,10f,0));
        lr.SetPosition(2, new Vector3(2f,2f,0));
    }
}
