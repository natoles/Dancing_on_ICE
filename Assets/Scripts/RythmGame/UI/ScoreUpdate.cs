using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{

    public GameObject textScore; 
    GameObject scoring; 
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Main");
    }

    // Update is called once per frame
    void Update()
    {
        textScore.GetComponent<Text>().text = "SCORE : " + (int)main.GetComponent<Scoring>().tmpScore;
    }
}
