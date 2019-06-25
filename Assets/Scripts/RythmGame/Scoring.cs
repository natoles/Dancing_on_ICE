using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public int score = 0; //Actual score
    public int tmpScore = 0; //Displayed score

    // If displayed score is less than real score, increment it
    void Update()
    {
        if(tmpScore < score - 151){
            tmpScore += 151;
        } else {
            if ((tmpScore < score))
            tmpScore += score-tmpScore;
        }
    }

    //in case of point multiplicator add it here
    public void AddScore(int scoreAdded){
        score += scoreAdded;
    }
}
