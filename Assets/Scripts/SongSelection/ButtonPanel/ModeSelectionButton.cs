using UnityEngine;
using System;
using UnityEngine.UI;
using DancingICE.Modes;
using DancingICE.RythmGame;

public class ModeSelectionButton : Button
{
    [Serializable]
    private class ModeContainer
    {
        public Mode mode = null;
        public bool showDifficultySlider = false;
        public GameObject buttonsToShow = null;
#if UNITY_EDITOR
        public bool previousButtonsState = false;
#endif
    }

    [SerializeField]
    private Text textComponent = null;

    [SerializeField]
    private GameObject difficultySlider = null;

    [SerializeField]
    private ModeContainer[] modes = null;

    [SerializeField]
    private int current = 0;

    private ModeContainer lastDisplayedMode = null;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(NextMode);
    }

    protected override void Start()
    {
        base.Start();
        UpdateModeDisplay();
    }

    private void NextMode()
    {
        if (modes == null || modes.Length == 0)
            current = 0;
        else
        {
            int previous = current;
            current = (current + 1) % modes.Length;
            UpdateModeDisplay();
        }
    }

    public void UpdateModeDisplay()
    {
        current = Mathf.Clamp(current, 0, modes.Length - 1);
        RythmGameSettings.GameMode = modes[current].mode;
        UpdateButtonsVisibility();
        SetSliderVisibility();
        UpdateText();
        lastDisplayedMode = modes[current];
    }

    private void UpdateText()
    {
        if (textComponent != null)
        {
            string display = null;
            if (modes[current]?.mode != null)
            {
                if (modes[current].mode.useCustomName)
                    display = modes[current].mode.customName;
                else
                    display = modes[current].mode.name;
            }
            textComponent.text = display;
        }
    }

    private void UpdateButtonsVisibility()
    {
        if (lastDisplayedMode?.buttonsToShow != null)
            lastDisplayedMode.buttonsToShow.SetActive(false);

        if (current >= 0 && current < modes.Length && modes[current]?.buttonsToShow != null)
            modes[current].buttonsToShow.SetActive(true);
    }

    private void SetSliderVisibility()
    {
        if (current >= 0 && current < modes.Length && modes[current] != null && difficultySlider != null)
            difficultySlider.SetActive(modes[current].showDifficultySlider);
    }
}
