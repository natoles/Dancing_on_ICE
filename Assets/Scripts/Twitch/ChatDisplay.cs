using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Api.Models.Helix.Streams.GetStreams;
using UnityEngine;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour
{
    private TwitchClient client = null;
    private TwitchAPI api = null;
    private Text chat = null;
    public InputField twitchChannelInputField = null;
    public Button connectButton = null;
    

    private void Start()
    {
        client = TwitchClient.Instance;
        api = TwitchAPI.Instance;
        chat = GetComponent<Text>();
        client.OnMessageReceived += Chat_OnMessageReceived;
        twitchChannelInputField.onValueChanged.AddListener(delegate { ConnectLayout(); });
        connectButton.onClick.AddListener(ConnectToChannelWrapper);
    }

    private void ConnectToChannelWrapper()
    {
        StartCoroutine(ConnectToChannel());
    }

    private IEnumerator ConnectToChannel()
    {
        GetStreamsResponse obj = null;
        yield return
            api.InvokeAsync(
                api.Streams.helix.GetStreamsAsync(userLogins: new List<string> { twitchChannelInputField.text }),
                (response) => obj = response
                );
        
        if (obj.Streams.Length > 0)
        {
            Debug.Log($"{obj.Streams[0].ViewerCount} viewers on stream");
            client.ConnectTo(twitchChannelInputField.text);
            Text buttonText = connectButton.GetComponentInChildren<Text>();
            client.OnJoinedChannel += (sender, e) => { ConnectedLayout(); } ;
        }
        else
        {
            Debug.Log("User not live");
            twitchChannelInputField.textComponent.color = Color.red;
        }
    }

    private void ConnectLayout()
    {
        twitchChannelInputField.textComponent.color = Color.black;
        Text buttonText = connectButton.GetComponentInChildren<Text>();
        buttonText.text = "Connect";
        buttonText.color = Color.black;
        connectButton.interactable = true;
    }

    private void ConnectedLayout()
    {
        twitchChannelInputField.textComponent.color = Color.black;
        Text buttonText = connectButton.GetComponentInChildren<Text>();
        buttonText.text = "Connected";
        buttonText.color = Color.green;
        connectButton.interactable = false;
    }

    private void Chat_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        chat.text += "\n" + e.ChatMessage.Username + ": " + e.ChatMessage.Message;
    }
}
