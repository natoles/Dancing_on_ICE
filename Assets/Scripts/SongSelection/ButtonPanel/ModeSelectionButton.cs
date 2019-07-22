using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class ModeSelectionButton : Button
{
    [Serializable]
    class ModeContainer
    {
        public string name = null;
        public GameObject buttons = null;
    }

    [SerializeField]
    private int current = 0;

    [SerializeField]
    private Text TextComponent = null;

    [SerializeField]
    private ModeContainer[] Modes = null;
    
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(NextMode);
    }

    protected override void Start()
    {
        base.Start();
        UpdateText();
    }

    private void NextMode()
    {
        int previous = current;
        current = (current + 1) % Modes.Length;
        UpdateModeDisplay();
    }

    public void UpdateModeDisplay()
    {
        UpdateButtonsVisibility();
        UpdateText();
    }

    private void UpdateText()
    {
        if (TextComponent != null)
            TextComponent.text = Modes[current]?.name + " Mode";
    }

    private void UpdateButtonsVisibility()
    {
        for (int i = 0; i < Modes.Length; ++i)
            SetButtonsVisibility(i, i == current);
    }

    private void SetButtonsVisibility(int id, bool visible)
    {
        if (Modes[id] != null && Modes[id].buttons != null)
            Modes[id].buttons.SetActive(visible);
    }
}
