using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScrollRect : ScrollRect
{
    private OptionEntry prefab = null;

    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying)
        {
            prefab = GetComponentInChildren<OptionEntry>();
            DisplayOptions();
        }
    }

    private void DisplayOptions()
    {
        PropertyInfo[] properties = typeof(SettingsManager.Twitch).GetProperties();
        foreach (PropertyInfo pif in properties)
        {
            OptionEntry opt = Instantiate(prefab, content.transform);
            opt.Property = pif;
        }
    }
}
