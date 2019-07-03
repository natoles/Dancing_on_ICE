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
    protected SettingTyp m_SettingType = SettingTyp.AudTimCmd;

    protected override void Start()
    {
        if (Application.isPlaying) // prevent this part from being executed in EditMode (because TwitchClient is not instancied at this time)
        {
            text = SettingsManager.Instance.twitch[m_SettingType].value;
            onValueChanged.AddListener(UpdateCommand);
        }
    }

    private void UpdateCommand(string arg0)
    {
        SettingsManager.Instance.twitch[m_SettingType].value = arg0;
    }
}
