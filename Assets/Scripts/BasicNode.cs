using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNode : MonoBehaviour
{
    GameObject movingPart;
    GameObject goal;
    GameObject rightHand;
    float speed = 1f;
    float size;
    public GameObject hand;
    bool inCircle = false;
    // Start is called before the first frame update
    void Start()
    {
        goal = this.transform.GetChild(0).gameObject;
        movingPart = this.transform.GetChild(1).gameObject;
        rightHand = GameObject.Find("RightHand");
        size = goal.transform.lossyScale.x;
        hand.GetComponent<Rigidbody2D>().position = goal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movingPart.transform.localScale += new Vector3(speed/100, speed/100, 0);
        //Debug.Log(size);
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
        Debug.Log(col.gameObject);
        if (col.gameObject == hand){
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
