using System;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySlider : Slider
{
    [SerializeField]
    private Text text = null;

    protected override void Start()
    {
        base.Start();

        if (text != null)
            value = RythmGameSettings.Difficulty;
    }

    protected override void Update()
    {
        base.Update();
        if (text != null)
        {
            RythmGameSettings.Difficulty = value;
            value = RythmGameSettings.Difficulty; // to be sure we submitted a valid value
            text.text = $"Difficulty: {Math.Round(value, 1).ToString("0.0")}";
        }
    }
}
