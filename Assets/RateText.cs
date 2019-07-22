using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RateText : MonoBehaviour
{
    public MainCreator main;
    public GameObject rateText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = (float) Math.Floor(main.currentRates[1] * 10)/10;
        rateText.GetComponent<Text>().text = "L: " + x  + "; R: " + (100-x);
    }
}
