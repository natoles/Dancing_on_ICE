using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
/* 
[CustomEditor(typeof(SettingField))]
public class SettingFieldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}*/


[Serializable]
public class SettingField : InputField
{
    [SerializeField]
    protected SettingTyp type = SettingTyp.AudTimCmd;

    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying) // prevent this part from being executed in EditMode (because TwitchClient is not instancied at this time)
        {
            text = SettingsManager.Instance.twitch[type].value;
            onValueChanged.AddListener(UpdateCommand);
        }
    }

    private void UpdateCommand(string arg0)
    {
        SettingsManager.Instance.twitch[type].value = arg0;
    }
}
