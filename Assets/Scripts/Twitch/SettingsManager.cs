using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;

public enum SettingTyp
{
    TwitchUsr,  // Twitch Username
    AudTimCmd,  // Audience Time
    IncDiffCmd, // Increase difficulty
    DecDiffCmd, // Decrease difficulty
    CongratCmd  // Congratulate
}

public class SettingsManager : Singleton<SettingsManager>
{
    // Twitch-related settings
    private readonly string twitchSection = "Twitch";
    public readonly Dictionary<SettingTyp, StringSetting> twitch = new Dictionary<SettingTyp, StringSetting>
    {
        //  Command Type                                Config file field               Default value
        {   SettingTyp.TwitchUsr,   new StringSetting(  "TwitchUsername"            ,   ""            ) },
        {   SettingTyp.AudTimCmd,   new StringSetting(  "AudienceTimeCommand"       ,   "gambatte"    ) },
        {   SettingTyp.IncDiffCmd,  new StringSetting(  "IncreaseDifficultyCommand" ,   "increase"    ) },
        {   SettingTyp.DecDiffCmd,  new StringSetting(  "DecreaseDifficultyCommand" ,   "decrease"    ) },
        {   SettingTyp.CongratCmd,  new StringSetting(  "CongratulationCommand"     ,   "gg"          ) },
    };

    #region Implementation
    private string path; // setting file path

    public class StringSetting
    {
        internal string setting;
        public string value;

        public StringSetting(string s, string v)
        {
            setting = s;
            value = v;
        }
    }

    private void Awake()
    {
        path = $"{Application.persistentDataPath}/config.cfg";
        if (System.IO.File.Exists(path))
        {
            Configuration config = Configuration.LoadFromFile(path);
            
            foreach (KeyValuePair<SettingTyp, StringSetting> c in twitch)
            {
                SettingTyp typ = c.Key;
                c.Value.value = config[twitchSection][c.Value.setting].StringValue;
            }
        }
    }

    private void SaveConfig()
    {
        Configuration config = new Configuration();
        foreach (KeyValuePair<SettingTyp, StringSetting> c in twitch)
        {
            SettingTyp typ = c.Key;
            config[twitchSection][c.Value.setting].StringValue = c.Value.value;
        }
        config.SaveToFile(path);
    }

    private void OnApplicationQuit()
    {
        SaveConfig();
    }

    private void OnDestroy()
    {
        SaveConfig();
    }

    #endregion
}
