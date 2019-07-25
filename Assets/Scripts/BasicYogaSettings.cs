using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BasicYogaSettings : MonoBehaviour
{
    [SerializeField]
    Slider slider = null;

    [SerializeField]
    Text leftValue = null;

    [SerializeField]
    Text rightValue = null;

    [SerializeField]
    Toggle basic = null;

    [SerializeField]
    Toggle line = null;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = MainCreator.wantedRates[1];
        leftValue.text = slider.value.ToString();
        rightValue.text = (100 - slider.value).ToString();

        basic.isOn = MainCreator.globalNodeType == MainCreator.Mode.Basic;
        line.isOn = MainCreator.globalNodeType == MainCreator.Mode.Line;
    }

    // Update is called once per frame
    void Update()
    {
        MainCreator.wantedRates[0] = 100 - slider.value;
        MainCreator.wantedRates[1] = slider.value;

        leftValue.text = slider.value.ToString();
        rightValue.text = (100 - slider.value).ToString();

        if (basic.isOn)
            MainCreator.globalNodeType = MainCreator.Mode.Basic;
        else if (line.isOn)
            MainCreator.globalNodeType = MainCreator.Mode.Basic;
    }
}
