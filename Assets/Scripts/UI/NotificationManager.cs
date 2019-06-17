using System;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : Singleton<NotificationManager>
{
    public NotificationPanel notificationPanelPrefab;

    private string channel = null;

    bool quitting = false;

    protected void Start()
    {
        TwitchClient.Instance.OnJoinedChannel += Notifier_OnJoinedChannel;
        TwitchClient.Instance.OnLeftChannel += Notifier_OnLeftChannel;
        TwitchClient.Instance.OnDisconnected += Notifier_OnDisconnected;
    }

    public void PushNotification(string text, Color textColor, Color bgColor)
    {
        NotificationPanel notif = Instantiate<NotificationPanel>(notificationPanelPrefab);
        notif.name = "NotificationPanel";
        notif.rectTransform.SetParent(transform);
        notif.textComponent.text = text;
        notif.textComponent.color = textColor;
        notif.color = bgColor;
        notif.Trigger();
    }

    private void Notifier_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
    {
        PushNotification("Connected to channel " + e.Channel, Color.white, Color.green);
        channel = e.Channel;
    }

    private void Notifier_OnLeftChannel(object sender, TwitchLib.Client.Events.OnLeftChannelArgs e)
    {
        if (channel != null)
        {
            PushNotification("Disconnected from channel " + channel, Color.white, Color.red);
            channel = null;
        }
    }

    private void Notifier_OnDisconnected(object sender, OnDisconnectedArgs e)
    {
        if (channel != null)
        {
            PushNotification("Disconnected from channel " + channel, Color.white, Color.red);
            channel = null;
        }
    }

    private void OnApplicationQuit()
    {
        quitting = true;
    }

    protected void OnDestroy()
    {
        if (!quitting) // prevent this part from being executed in EditMode or while exiting (because TwitchClient may be not instancied at this time)
        {
            TwitchClient.Instance.OnJoinedChannel -= Notifier_OnJoinedChannel;
            TwitchClient.Instance.OnLeftChannel -= Notifier_OnLeftChannel;
            TwitchClient.Instance.OnDisconnected -= Notifier_OnDisconnected;
        }
    }
}
