using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public int scoreUpdateTickValue = 127; //tmpScore is incresed by this value each tick
    public float scoreUpdateMaxDelay = 1f; //updating tmpScore cannot take longer than this delay

    public float score = 0; //Actual score
    public float tmpScore = 0; //Displayed score
    public float multiplier = 1f; //Score multiplier
    
    private float targetTimeUpdated = 0f; //time when tmpScore must have been updated
    private bool updated = true; //is tmpScore updated? (ie. score == tmpScore)

    // If displayed score is less than real score, increment it
    void Update()
    {
        if (!updated)
        {
            tmpScore = Mathf.Min(score, tmpScore + Mathf.Max(scoreUpdateTickValue, (score - tmpScore) * Time.deltaTime / (targetTimeUpdated - Time.time)));

            if (Time.time >= targetTimeUpdated)
            {
                tmpScore = score;
                updated = true;
            }
        }
    }

    //in case of point multiplicator add it here
    public void AddScore(float scoreAdded){
        score += scoreAdded * multiplier;
        targetTimeUpdated = Time.time + scoreUpdateMaxDelay;
        updated = false;
    }
}
