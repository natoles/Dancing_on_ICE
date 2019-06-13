using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TwitchLib.Api.Models.Helix.Streams.GetStreams;
using System;
using TwitchLib.Client.Events;

public class ConnectButton : Button
{
    protected override void Awake()
    {
        base.Awake();

        if (Application.isPlaying)
        {
            onClick.AddListener(ConnectToStreamWrapper);
            TwitchClient.Instance.OnJoinedChannel += ConnectButton_OnJoinedChannel;
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
    }

    private void LayoutConnected()
    {
        Text text = GetComponentInChildren<Text>();
        text.text = "Connected";
        text.color = Color.green;
    }
}
