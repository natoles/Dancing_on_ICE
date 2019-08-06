using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Api.Models.Helix.Streams.GetStreams;
using UnityEngine;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour
{
    private Text chat = null;
    public InputField twitchChannelInputField = null;
    public ConnectButton connectButton = null;

    private bool quitting = false;

    private void Start()
    {
        chat = GetComponent<Text>();
        TwitchClient.Instance.OnMessageReceived += Chat_OnMessageReceived;
        twitchChannelInputField.text = SettingsManager.Twitch.TwitchUsername;
        twitchChannelInputField.onValueChanged.AddListener(delegate { connectButton.ChangeButtonLayout("Connect", Color.green, true); });
    }

    private void Chat_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        chat.text += "\n" + e.ChatMessage.Username + ": " + e.ChatMessage.Message;
    }

    private void OnApplicationQuit()
    {
        quitting = true;
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && !quitting)
            TwitchClient.Instance.OnMessageReceived -= Chat_OnMessageReceived;
    }
}
