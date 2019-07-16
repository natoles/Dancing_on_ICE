using System;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySlider : Slider
{
    [SerializeField]
    private Text text = null;
    
    protected override void Update()
    {
        base.Update();
        if (text != null)
        {
            text.text = $"Difficulty: {Math.Round(value, 1).ToString("0.0")}";
            TwitchRythmController.Difficulty = value;
        }
    }
}
