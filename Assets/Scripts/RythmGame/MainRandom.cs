using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRandom : MonoBehaviour
{
    bool createNode = true;

    //Nodes list
    public GameObject BasicNodeHand; 
    public GameObject BasicNodeRightHand;
    public GameObject BasicNodeLeftHand;
    public GameObject LineNodeRightHand;
    public GameObject LineNodeLeftHand;
    public float spawnInterval = 2.5f;
    

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
    }

    IEnumerator BasicNodeCreation()
    {
        createNode = false;
        
        //Spawn a random node
        int type = Random.Range(1,6);
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
            case 4 : 
                GameObject newLineNodeRightHand = Instantiate(LineNodeRightHand, RandomPointInBounds(RH_zone.bounds), Quaternion.Euler(0,0,0));
                break;
            case 5 : 
                GameObject newLineNodeLeftHand = Instantiate(LineNodeLeftHand, RandomPointInBounds(LH_zone.bounds), Quaternion.Euler(0,0,0));
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
