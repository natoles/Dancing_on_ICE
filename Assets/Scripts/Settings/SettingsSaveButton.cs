﻿using UnityEngine;
using UnityEngine.UI;

public class SettingsSaveButton : Button
{
    protected override void Awake()
    {
        onClick.AddListener(SaveSettings);
    }

    private void SaveSettings()
    {
        SettingsManager.Save();
        NotificationManager.Instance.PushNotification("Configuration saved", Color.white, Color.green);
    }
}
