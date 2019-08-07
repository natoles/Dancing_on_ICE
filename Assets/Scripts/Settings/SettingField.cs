using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[Serializable]
public class SettingField : InputField
{
    [SerializeField]
    protected string section;

    [SerializeField]
    protected string optionName;
    
    protected override void Start()
    {
        if (Application.isPlaying) // prevent this part from being executed in EditMode (because TwitchClient is not instancied at this time)
        {
            text = SettingsManager.GetValue<string>(section, optionName);
            onValueChanged.AddListener(UpdateCommand);
        }
    }

    private void UpdateCommand(string arg0)
    {
        SettingsManager.SetValue(section, optionName, arg0);
    }
}
