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

    private int current = 0;

    [SerializeField]
    private Text TextComponent = null;

    [SerializeField]
    private ModeContainer[] Modes = null;
    
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(SelectMode);
    }

    private void SelectMode()
    {
        int previous = current;
        current = (current + 1) % Modes.Length;
        if (TextComponent != null)
        {
            TextComponent.text = Modes[current]?.name;
        }

        Modes[previous]?.buttons?.SetActive(false);
        Modes[current]?.buttons?.SetActive(true);
    }
}
