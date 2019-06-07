using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    bool createNode = true;
    public GameObject BasicNodeHand; 
    public GameObject BasicNodeRightHand;
    public GameObject BasicNodeLeftHand;
    int xPos;
    int yPos;
    float spawnInterval = 2f;
    public int Score = 0;
    public int tmpScore = 0;
    
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
        xPos = Random.Range(-17, 17);
        yPos = Random.Range(-9, 9);
        int type = Random.Range(1,4);
        switch (type)
        {
            case 1 : 
                GameObject newBasicNodeHand = Instantiate(BasicNodeHand, new Vector3(xPos, yPos,0), Quaternion.Euler(0,0,0));
                break;
            case 2 : 
                GameObject newBasicNodeRightHand = Instantiate(BasicNodeRightHand, new Vector3(xPos, yPos,0), Quaternion.Euler(0,0,0));
                break;
            case 3 : 
                GameObject newBasicNodeLeftHand = Instantiate(BasicNodeLeftHand, new Vector3(xPos, yPos,0), Quaternion.Euler(0,0,0));
                break;
            default :
                GameObject newBasicNodeDefault = Instantiate(BasicNodeHand, new Vector3(xPos, yPos,0), Quaternion.Euler(0,0,0));
                break;
        }
        
        yield return new WaitForSeconds(spawnInterval);
        createNode = true;
        
    }
}
