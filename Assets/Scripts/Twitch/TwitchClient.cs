using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitch = TwitchLib.Unity;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using System;

public class TwitchClient : Twitch.Client
{
    private static readonly TwitchClient instance = new TwitchClient();
    private static readonly ConnectionCredentials credentials = new ConnectionCredentials(AuthTokens.BOT_NAME, AuthTokens.BOT_ACCESS_TOKEN);

    private TwitchClient() {}
    
    public static TwitchClient Instance()
    {
        return instance;
    }

    public void Connect(string channelToJoin)
    {
        base.Initialize(credentials, channelToJoin);
        base.Connect();
    }

    private new void Connect() {}
}
