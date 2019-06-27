using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;

public class TwitchScoring : Scoring
{

    private void Start()
    {
        TwitchClient.Instance.OnMessageReceived += Increase_Score;
    }

    private void Increase_Score(object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message == SettingsManager.Instance.twitch[SettingTyp.AudTimCmd].value)
        {
            multiplier += 0.1f;
        }
    }
}