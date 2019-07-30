using SharpConfig;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScrollRect : ScrollRect
{
    private string[] sections = new string[] { "twitch" };
    
    private OptionEntry prefab = null;

    protected override void Start()
    {
        prefab = GetComponentInChildren<OptionEntry>();
        base.Start();
        if (Application.isPlaying)
        {
            foreach (string section in sections)
            {
                DisplayOptions(section);
            }
        }
    }

    private void DisplayOptions(string section)
    {
        foreach (Setting s in SettingsManager.Instance.config[section])
        {
            OptionEntry opt = Instantiate<OptionEntry>(prefab, content.transform);
            opt.OptionName = s.Name;
            opt.SetLayout(typeof(string));
            opt.StringValue = s.StringValue;
        }
    }
}
