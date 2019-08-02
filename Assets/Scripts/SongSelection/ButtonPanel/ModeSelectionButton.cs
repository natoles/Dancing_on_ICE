using UnityEngine;
using System;
using UnityEngine.UI;
using DancingICE.Modes;

public class ModeSelectionButton : Button
{
    [Serializable]
    private class ModeContainer
    {
        public Mode mode = null;
        public bool showDifficultySlider = false;
        public GameObject buttonsToShow = null;
    }

    [SerializeField]
    private Text textComponent = null;

    [SerializeField]
    private GameObject difficultySlider = null;

    [SerializeField]
    private ModeContainer[] modes = null;

    [SerializeField]
    private int current = 0;

    public int Current
    {
        get => current;
        set
        {
            if (modes == null || modes.Length == 0)
                current = 0;
            else
            {
                int previous = current;
                current = value % modes.Length;
                UpdateModeDisplay(previous, current);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(NextMode);
    }

    protected override void Start()
    {
        base.Start();
        UpdateText(current);
    }

    private void NextMode()
    {
        if (modes == null || modes.Length == 0)
            current = 0;
        else
        {
            int previous = current;
            current = (current + 1) % modes.Length;
            UpdateModeDisplay(previous, current);
        }
    }

    private void UpdateModeDisplay(int previous, int current)
    {
        UpdateButtonsVisibility(previous, current);
        SetSliderVisibility(current);
        UpdateText(current);
    }

    private void UpdateText(int current)
    {
        if (textComponent != null)
        {
            string display = null;
            if (modes[current].mode != null)
            {
                if (modes[current].mode.useCustomName)
                    display = modes[current].mode.customName;
                else
                    display = modes[current].mode.name;
            }
            textComponent.text = display;
        }
    }

    private void UpdateButtonsVisibility(int previous, int current)
    {
        if (modes[previous].buttonsToShow != null)
            modes[previous].buttonsToShow.SetActive(false);

        if (modes[current].buttonsToShow != null)
            modes[current].buttonsToShow.SetActive(true);
    }

    private void SetSliderVisibility(int current)
    {
        difficultySlider.SetActive(modes[current].showDifficultySlider);
    }
}
