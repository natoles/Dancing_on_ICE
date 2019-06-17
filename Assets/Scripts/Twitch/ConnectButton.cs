using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TwitchLib.Api.Models.Helix.Streams.GetStreams;
using System;
using TwitchLib.Client.Events;

public class ConnectButton : Button
{
    private bool quitting = false;
    private Text text = null;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<Text>();
        ChangeButtonLayout("Connect", Color.green);

        if (Application.isPlaying) // prevent this part from being executed in EditMode (because TwitchClient is not instancied at this time)
        {
            onClick.AddListener(ConnectButton_OnClickHandler);
            TwitchClient.Instance.OnJoinedChannel += ConnectButton_OnJoinedChannel;
            TwitchClient.Instance.OnLeftChannel += ConnectButton_OnLeftChannel;
            TwitchClient.Instance.OnDisconnected += ConnectButton_OnDisconnected;
            if (TwitchClient.Instance.IsConnected)
                ChangeButtonLayout("Disconnect", Color.red);
        }
    }

    private void ConnectButton_OnClickHandler()
    {
        string user = SettingsManager.Instance.twitch[SettingTyp.TwitchUsr].value;
        if (user == null || user == string.Empty)
        {
            return;
        }

        interactable = false;
        if (!TwitchClient.Instance.IsConnected)
        {
            ChangeButtonLayout("Connecting", Color.grey);
            StartCoroutine(ConnectToStream(user));
        }
        else
        {
            ChangeButtonLayout("Disconnecting", Color.grey);
            TwitchClient.Instance.Disconnect();
        }
    }

    private IEnumerator ConnectToStream(string stream)
    {
        GetStreamsResponse obj = null;
        yield return
            TwitchAPI.Instance.InvokeAsync(
                TwitchAPI.Instance.Streams.helix.GetStreamsAsync(userLogins: new List<string> { stream }),
                (response) => obj = response
                );

        if (obj.Streams.Length > 0)
        {
            Debug.Log($"{obj.Streams[0].ViewerCount} viewers on stream");
            TwitchClient.Instance.ConnectTo(stream);
        }
        else
        {
            Debug.Log("User not live");
            ChangeButtonLayout("Connect", Color.green);
            interactable = true;
        }
    }

    private void ConnectButton_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        ChangeButtonLayout("Disconnect", Color.red);
        interactable = true;
    }

    private void ConnectButton_OnLeftChannel(object sender, OnLeftChannelArgs e)
    {
        ChangeButtonLayout("Connect", Color.green);
        interactable = true;
    }

    private void ConnectButton_OnDisconnected(object sender, OnDisconnectedArgs e)
    {
        ChangeButtonLayout("Connect", Color.green);
        interactable = true;
    }

    private void ChangeButtonLayout(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            string user = SettingsManager.Instance.twitch[SettingTyp.TwitchUsr].value;
            if (user == null || user == string.Empty)
            {
                interactable = false;
            }
            else
            {
                interactable = true;
            }
        }
    }

    private void OnApplicationQuit()
    {
        quitting = true;
    }

    protected override void OnDestroy()
    {
        if (Application.isPlaying && !quitting) // prevent this part from being executed in EditMode or while exiting (because TwitchClient may be not instancied at this time)
        {
            TwitchClient.Instance.OnJoinedChannel -= ConnectButton_OnJoinedChannel;
            TwitchClient.Instance.OnLeftChannel -= ConnectButton_OnLeftChannel;
            TwitchClient.Instance.OnDisconnected -= ConnectButton_OnDisconnected;
        }
        base.OnDestroy();
    }
}
