using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreation : MonoBehaviour
{
    bool createNode = true;
    public GameObject BasicNodeHand; 
    int xPos;
    int yPos;
    float spawnInterval = 2f;
    
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
        xPos = Random.Range(-17, 17);
        yPos = Random.Range(-9, 9);
        GameObject newBasicNode = Instantiate(BasicNodeHand, new Vector3(xPos, yPos,0), Quaternion.Euler(0,0,0));
        newBasicNode.transform.localScale = new Vector3(1f,1f,1f); //Changes the scale
        yield return new WaitForSeconds(spawnInterval);
        createNode = true;
        
    }
}
