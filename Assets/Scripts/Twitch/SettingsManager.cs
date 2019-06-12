using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConfig;

public class SettingsManager : Singleton<SettingsManager>
{
    public enum CommandType
    {
        AudienceTime,
        IncreaseDifficulty,
        DecreaseDifficulty,
        Congratulate
    }

    private string path;
    public Configuration config = null;

    private void Awake()
    {
        path = $"{Application.persistentDataPath}/config.cfg";
        Debug.Log(path);
        if (System.IO.File.Exists(path))
        {
            config = Configuration.LoadFromFile(path);
        }
        else
        {
            config = new Configuration();
            config["ChatCommands"]["AudienceTimeCommand"]       .StringValue = "gambatte";
            config["ChatCommands"]["IncreaseDifficultyCommand"] .StringValue = "increase";
            config["ChatCommands"]["DecreaseDifficultyCommand"] .StringValue = "decrease";
            config["ChatCommands"]["CongratulationCommand"]     .StringValue = "gg";
            SaveConfig();
        }
    }

    private void SaveConfig()
    {
        config.SaveToFile(path);
        Debug.Log(config);
    }

    private void OnApplicationQuit()
    {
        SaveConfig();
    }

    private void OnDestroy()
    {
        SaveConfig();
    }
}
