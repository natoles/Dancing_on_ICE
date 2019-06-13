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

    protected override void Awake()
    {
        base.Awake();

        if (Application.isPlaying) // prevent this part from being executed in EditMode (because TwitchClient is not instancied at this time)
        {
            onClick.AddListener(ConnectToStreamWrapper);
            TwitchClient.Instance.OnJoinedChannel += ConnectButton_OnJoinedChannel;
            TwitchClient.Instance.OnLeftChannel += ConnectButton_OnLeftChannel;
            if (TwitchClient.Instance.IsConnected)
                LayoutConnected();
        }
    }

    private void ConnectToStreamWrapper()
    {
        StartCoroutine(ConnectToStream());
    }

    private IEnumerator ConnectToStream()
    {
        string user = SettingsManager.Instance.twitch[SettingTyp.TwitchUsr].value;
        GetStreamsResponse obj = null;
        yield return
            TwitchAPI.Instance.InvokeAsync(
                TwitchAPI.Instance.Streams.helix.GetStreamsAsync(userLogins: new List<string> { user }),
                (response) => obj = response
                );

        if (obj.Streams.Length > 0)
        {
            Debug.Log($"{obj.Streams[0].ViewerCount} viewers on stream");
            TwitchClient.Instance.ConnectTo(user);
        }
        else
        {
            Debug.Log("User not live");
        }
    }

    private void ConnectButton_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        LayoutConnected();
        interactable = false;
    }

    private void ConnectButton_OnLeftChannel(object sender, OnLeftChannelArgs e)
    {
        LayoutDisconnected();
        interactable = true;
    }

    private void LayoutDisconnected()
    {
        Text text = GetComponentInChildren<Text>();
        text.text = "Connect";
        text.color = Color.black;
    }

    private void LayoutConnected()
    {
        Text text = GetComponentInChildren<Text>();
        text.text = "Connected";
        text.color = Color.green;
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
        }
        base.OnDestroy();
    }
}
