using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalText : MonoBehaviour
{
    public BodySourceView bodyView;
    public GameObject CaloriesText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int x = bodyView.calories;
        if (bodyView.caloriesActive){
            CaloriesText.GetComponent<Text>().text = "KCal: " + x;
        } else CaloriesText.GetComponent<Text>().text = "";

    }
}
