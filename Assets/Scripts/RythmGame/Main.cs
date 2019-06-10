using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    bool createNode = true;

    //Nodes list
    public GameObject BasicNodeHand; 
    public GameObject BasicNodeRightHand;
    public GameObject BasicNodeLeftHand;
    float spawnInterval = 1f;
    public int Score = 0; //Actual score
    public int tmpScore = 0; //Score displayed

    //Zones for node spawn
    public Collider LH_zone; 
    public Collider RH_zone; 
    public Collider H_zone; 
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (createNode)
            StartCoroutine(BasicNodeCreation());

        //Score update
        if(tmpScore < Score - 151){
            tmpScore += 151;
        } else {
            if ((tmpScore < Score))
            tmpScore += Score-tmpScore;
        }

    }

    IEnumerator BasicNodeCreation()
    {
        createNode = false;
        
        //Spawn a random node
        int type = Random.Range(1,4);
        switch (type)
        {
            case 1 : 
                GameObject newBasicNodeHand = Instantiate(BasicNodeHand, RandomPointInBounds(H_zone.bounds), Quaternion.Euler(0,0,0));
                break;
            case 2 : 
                GameObject newBasicNodeRightHand = Instantiate(BasicNodeRightHand, RandomPointInBounds(RH_zone.bounds), Quaternion.Euler(0,0,0));
                break;
            case 3 : 
                GameObject newBasicNodeLeftHand = Instantiate(BasicNodeLeftHand, RandomPointInBounds(LH_zone.bounds), Quaternion.Euler(0,0,0));
                break;
            default :
                GameObject newBasicNodeDefault = Instantiate(BasicNodeHand, RandomPointInBounds(H_zone.bounds), Quaternion.Euler(0,0,0));
                break;
        }
        
        yield return new WaitForSeconds(spawnInterval);
        createNode = true;
    }

    //Return a random Vector3 inside a collider
    public static Vector3 RandomPointInBounds(Bounds bounds) {
    return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        0
    );
    }
}
