using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    bool createNode = true;
    public GameObject BasicNodeHand; 
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
        GameObject newBasicNode = Instantiate(BasicNodeHand, new Vector3(xPos, yPos,0), Quaternion.Euler(0,0,0));
        newBasicNode.transform.localScale = new Vector3(1f,1f,1f); //Changes the scale
        yield return new WaitForSeconds(spawnInterval);
        createNode = true;
        
    }
}
