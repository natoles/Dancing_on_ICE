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
        interactable = false;
        base.Awake();
        
        text = GetComponentInChildren<Text>();
        ChangeButtonLayout("Connect", Color.green, true);

        if (Application.isPlaying) // prevent this part from being executed in EditMode (because TwitchClient is not instancied at this time)
        {
            onClick.AddListener(ConnectButton_OnClickHandler);
            TwitchClient.Instance.OnJoinedChannel += ConnectButton_OnJoinedChannel;
            TwitchClient.Instance.OnLeftChannel += ConnectButton_OnLeftChannel;
            TwitchClient.Instance.OnDisconnected += ConnectButton_OnDisconnected;
            TwitchClient.Instance.OnFailureToReceiveJoinConfirmation += ConnectButton_OnJoinFailed;
            if (TwitchClient.Instance.IsConnected)
                ChangeButtonLayout("Disconnect", Color.red, true);
        }

        interactable = true;
    }

    private void ConnectButton_OnClickHandler()
    {
        string channel = SettingsManager.Instance.twitch[SettingTyp.TwitchUsr].value;

        if (!TwitchClient.Instance.IsConnected)
        {
            if (TwitchClient.Instance.JoinChannel(channel))
            {
                ChangeButtonLayout("Connecting", Color.grey, false);
            }
        }
        else
        {
            if (TwitchClient.Instance.LeaveChannel())
                ChangeButtonLayout("Disconnecting", Color.grey, false);
        }
    }

    //private IEnumerator ConnectToStream(string stream)
    //{
    //    GetStreamsResponse obj = null;
    //    yield return
    //        TwitchAPI.Instance.InvokeAsync(
    //            TwitchAPI.Instance.Streams.helix.GetStreamsAsync(userLogins: new List<string> { stream }),
    //            (response) => obj = response
    //            );

    //    if (obj.Streams.Length > 0)
    //    {
    //        Debug.Log($"{obj.Streams[0].ViewerCount} viewers on stream");
    //        TwitchClient.Instance.JoinChannel(stream);
    //    }
    //    else
    //    {
    //        Debug.Log("User not live");
    //        NotificationManager.Instance.PushNotification("User not live", Color.white, Color.blue);
    //        ChangeButtonLayout("Connect", Color.green, true);
    //    }
    //}

    private void ConnectButton_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        ChangeButtonLayout("Disconnect", Color.red, true);
    }

    private void ConnectButton_OnLeftChannel(object sender, OnLeftChannelArgs e)
    {
        ChangeButtonLayout("Connect", Color.green, true);
    }

    private void ConnectButton_OnDisconnected(object sender, OnDisconnectedArgs e)
    {
        ChangeButtonLayout("Connect", Color.green, true);
    }

    private void ConnectButton_OnJoinFailed(object sender, OnFailureToReceiveJoinConfirmationArgs e)
    {
        ChangeButtonLayout("Connect", Color.green, true);
    }

    public void ChangeButtonLayout(string text, Color color, bool interactable)
    {
        this.text.text = text;
        this.text.color = color;
        this.interactable = interactable;
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
