using System;
using UnityEngine;
using SharpConfig;

public class SettingSectionAttribute : Attribute { internal SettingSectionAttribute() { } }

public class SettingsManager
{
    // Just add the settings you want to expose here
    // Classes containing settings must be marked as "static" with a "SettingSectionAttribute"

    [SettingSection]
    public static class Twitch
    {
        // Channel name (will join this chat)
        public static string TwitchUsername { get => GetValue<string>("Twitch", "TwitchUsername"); set => SetValue("Twitch", "TwitchUsername", value); }
        public static string AudienceTimeCommand { get => GetValue<string>("Twitch", "AudienceTimeCommand"); set => SetValue("Twitch", "AudienceTimeCommand", value); }
        public static string CongratulationCommand { get => GetValue<string>("Twitch", "CongratulationCommand"); set => SetValue("Twitch", "CongratulationCommand", value); }
    }

    [SettingSection]
    public static class Yoga
    {

    }

    #region Implementation

    private static SettingsManager instance = null;
    private static SettingsManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SettingsManager();
            return instance;
        }
    }

    private static string path = $"{Application.persistentDataPath}/config.cfg";
    private Configuration config = null;

    #region Get / Set

    public static T GetValue<T>(string section, string settingName)
    {
        return Instance.config[section][settingName].GetValue<T>();
    }

    public static object GetValue(string section, string settingName, Type type)
    {
        return Instance.config[section][settingName].GetValue(type);
    }

    public static void SetValue(string section, string settingName, object value)
    {
        Instance.config[section][settingName].SetValue(value);
    }

    #endregion

    #region Constructor

    private SettingsManager()
    {
        if (System.IO.File.Exists(path))
            config = Configuration.LoadFromFile(path);
        else
        {
            config = new Configuration();
        }
    }

    #endregion

    #region Save / Load

    public static void Load()
    {
        instance = new SettingsManager();
    }

    public static void Reload() => Load();

    public static void Save()
    {
        if (Instance.config == null)
            return;

        Instance.config.SaveToFile(path);
    }

    #endregion

    #endregion
}
