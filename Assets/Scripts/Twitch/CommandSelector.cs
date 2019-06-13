using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelector : MonoBehaviour
{
    InputField commandSelector = null;

    [SerializeField]
    public SettingTyp type = SettingTyp.AudTimCmd;

    private void Start()
    {
        commandSelector = GetComponent<InputField>();
        commandSelector.text = SettingsManager.Instance.twitch[type].value;
        commandSelector.onValueChanged.AddListener(UpdateCommand);
    }

    private void UpdateCommand(string arg0)
    {
        SettingsManager.Instance.twitch[type].value = arg0;
    }
}
