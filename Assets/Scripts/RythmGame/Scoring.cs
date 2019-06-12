using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public int Score = 0; //Actual score
    public int tmpScore = 0; //Score displayed
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tmpScore < Score - 151){
            tmpScore += 151;
        } else {
            if ((tmpScore < Score))
            tmpScore += Score-tmpScore;
        }
    }
}
