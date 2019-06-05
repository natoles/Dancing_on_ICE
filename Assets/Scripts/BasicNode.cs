using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart;
    GameObject goal;
    float speed = 1f;
    float size;
    public GameObject hand;
    bool inCircle = false;
    // Start is called before the first frame update
    void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        size = goal.transform.lossyScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        movingPart.transform.localScale += new Vector3(speed/100, speed/100, 0);
        if (movingPart.transform.localScale.x >= size){
            if (inCircle)
                {
                    Debug.Log("PERFECT");
                } else {
                    Debug.Log("MISSED");
                }
            Destroy(gameObject);
        } 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Bonjour");
        Debug.Log(col.gameObject.name);
        if (col.gameObject.name == "HandRight"){
            Debug.Log("I'm in");
            inCircle = true;
        } 
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == hand){
            inCircle = false;
        } 
    }

}
