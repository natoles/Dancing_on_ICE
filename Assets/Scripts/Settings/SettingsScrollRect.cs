using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScrollRect : ScrollRect
{
    [SerializeField]
    private OptionEntry prefab = null;
    
    [SerializeField]
    private string sectionTypeName = null;

    private List<OptionEntry> displayedSettings = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying)
            DisplayOptions();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Application.isPlaying)
        {
            if (displayedSettings != null)
            {
                foreach (OptionEntry s in displayedSettings)
                    Destroy(s.gameObject);
            }
        }
    }

    private void DisplayOptions()
    {
        displayedSettings = new List<OptionEntry>();
        PropertyInfo[] properties = Type.GetType(sectionTypeName, true, false).GetProperties();
        foreach (PropertyInfo pif in properties)
        {
            OptionEntry opt = Instantiate(prefab, content.transform);
            opt.Property = pif;
            displayedSettings.Add(opt);
        }
    }

    public void Apply()
    {
        foreach (OptionEntry s in displayedSettings)
            s.Apply();
        SettingsManager.Save();
    }

    public void Cancel()
    {
        foreach (OptionEntry s in displayedSettings)
            s.CancelModifications();
    }
}
