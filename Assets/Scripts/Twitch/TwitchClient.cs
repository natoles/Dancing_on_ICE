using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitch = TwitchLib.Unity;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;

public class TwitchClient : MonoBehaviour
{
    private Twitch.Client client;
    private string channelToJoin = "twifus123";

    private Text chat;

    private void Start()
    {
        chat = GetComponent<Text>();

        Application.runInBackground = true;

        ConnectionCredentials credentials = new ConnectionCredentials("dancing_on_ice", AuthTokens.BOT_ACCESS_TOKEN);
        client = new Twitch.Client();
        client.Initialize(credentials, channelToJoin);
        client.OnLog += Client_OnLog;
        client.OnMessageReceived += Client_OnMessageReceived;

        client.Connect();
    }

    private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        chat.text += "\n" + e.ChatMessage.Username + " : " + e.ChatMessage.Message;
    }

    private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
    {
        Debug.Log($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            client.SendMessage(client.JoinedChannels[0], "a-u");
        }
    }
}
