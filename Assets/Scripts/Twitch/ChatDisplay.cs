using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour
{
    private TwitchClient client;
    private Text chat = null;

    private void Start()
    {
        client = TwitchClient.Instance();
        chat = GetComponent<Text>();
        client.Connect("twifus123");
        client.OnMessageReceived += Chat_OnMessageReceived;
        client.OnChatCommandReceived += Chat_OnGGReceived;
    }

    private void Chat_OnGGReceived(object sender, OnChatCommandReceivedArgs e)
    {
        if (e.Command.CommandText.ToLower(System.Globalization.CultureInfo.InvariantCulture) == "gg")
        {
            Debug.Log($"{e.Command.ChatMessage.Username} sent you a GG !");
        }
    }

    private void Chat_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        chat.text += "\n" + e.ChatMessage.Username + ": " + e.ChatMessage.Message;
    }
    
}
