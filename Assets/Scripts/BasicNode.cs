using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart;
    GameObject goal;
    float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        movingPart.transform.localScale += new Vector3(speed/100, speed/100, 0);
    }
}
