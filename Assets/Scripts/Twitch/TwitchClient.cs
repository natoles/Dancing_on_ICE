using System;
using Twitch = TwitchLib.Unity;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using UnityEngine;

public class TwitchClient : Singleton<TwitchClient>
{
    private Twitch.Client client;
    private readonly ConnectionCredentials credentials;

    public event EventHandler<OnDisconnectedArgs> OnDisconnected;
    public event EventHandler<OnConnectionErrorArgs> OnConnectionError;
    public event EventHandler<OnChatClearedArgs> OnChatCleared;
    public event EventHandler<OnUserTimedoutArgs> OnUserTimedout;
    public event EventHandler<OnLeftChannelArgs> OnLeftChannel;
    public event EventHandler<OnUserBannedArgs> OnUserBanned;
    public event EventHandler<OnModeratorsReceivedArgs> OnModeratorsReceived;
    public event EventHandler<OnChatColorChangedArgs> OnChatColorChanged;
    public event EventHandler<OnSendReceiveDataArgs> OnSendReceiveData;
    public event EventHandler<OnNowHostingArgs> OnNowHosting;
    //public event EventHandler<OnBeingHostedArgs> OnBeingHosted; // Available only when connected as broadcaster
    public event EventHandler<OnRaidNotificationArgs> OnRaidNotification;
    public event EventHandler<OnGiftedSubscriptionArgs> OnGiftedSubscription;
    public event EventHandler OnSelfRaidError;
    public event EventHandler OnNoPermissionError;
    public event EventHandler OnRaidedChannelIsMatureAudience;
    public event EventHandler<OnRitualNewChatterArgs> OnRitualNewChatter;
    public event EventHandler<OnHostingStoppedArgs> OnHostingStopped;
    public event EventHandler<OnHostingStartedArgs> OnHostingStarted;
    public event EventHandler<OnUserLeftArgs> OnUserLeft;
    public event EventHandler<OnExistingUsersDetectedArgs> OnExistingUsersDetected;
    public event EventHandler<OnLogArgs> OnLog;
    public event EventHandler<OnConnectedArgs> OnConnected;
    public event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;
    public event EventHandler<OnIncorrectLoginArgs> OnIncorrectLogin;
    public event EventHandler<OnChannelStateChangedArgs> OnChannelStateChanged;
    public event EventHandler<OnUserStateChangedArgs> OnUserStateChanged;
    public event EventHandler<OnMessageReceivedArgs> OnMessageReceived;
    public event EventHandler<OnWhisperReceivedArgs> OnWhisperReceived;
    public event EventHandler<OnFailureToReceiveJoinConfirmationArgs> OnFailureToReceiveJoinConfirmation;
    public event EventHandler<OnMessageSentArgs> OnMessageSent;
    public event EventHandler<OnChatCommandReceivedArgs> OnChatCommandReceived;
    public event EventHandler<OnWhisperCommandReceivedArgs> OnWhisperCommandReceived;
    public event EventHandler<OnUserJoinedArgs> OnUserJoined;
    public event EventHandler<OnModeratorJoinedArgs> OnModeratorJoined;
    public event EventHandler<OnModeratorLeftArgs> OnModeratorLeft;
    public event EventHandler<OnNewSubscriberArgs> OnNewSubscriber;
    public event EventHandler<OnReSubscriberArgs> OnReSubscriber;
    public event EventHandler OnHostLeft;
    public event EventHandler<OnWhisperSentArgs> OnWhisperSent;
    public event EventHandler<OnUnaccountedForArgs> OnUnaccountedFor;

    private TwitchClient()
    {
        credentials = new ConnectionCredentials(AuthTokens.BOT_NAME, AuthTokens.BOT_ACCESS_TOKEN);
    }

    private void Awake()
    {
        client = new Twitch.Client();

        // Transfert events
        client.OnDisconnected                       += (s, e) => OnDisconnected                     ?.Invoke(this, e);
        client.OnConnectionError                    += (s, e) => OnConnectionError                  ?.Invoke(this, e);
        client.OnChatCleared                        += (s, e) => OnChatCleared                      ?.Invoke(this, e);
        client.OnUserTimedout                       += (s, e) => OnUserTimedout                     ?.Invoke(this, e);
        client.OnLeftChannel                        += (s, e) => OnLeftChannel                      ?.Invoke(this, e);
        client.OnUserBanned                         += (s, e) => OnUserBanned                       ?.Invoke(this, e);
        client.OnModeratorsReceived                 += (s, e) => OnModeratorsReceived               ?.Invoke(this, e);
        client.OnChatColorChanged                   += (s, e) => OnChatColorChanged                 ?.Invoke(this, e);
        client.OnSendReceiveData                    += (s, e) => OnSendReceiveData                  ?.Invoke(this, e);
        client.OnNowHosting                         += (s, e) => OnNowHosting                       ?.Invoke(this, e);
        //client.OnBeingHosted                        += (s, e) => OnBeingHosted                      ?.Invoke(this, e);
        client.OnRaidNotification                   += (s, e) => OnRaidNotification                 ?.Invoke(this, e);
        client.OnGiftedSubscription                 += (s, e) => OnGiftedSubscription               ?.Invoke(this, e);
        client.OnSelfRaidError                      += (s, e) => OnSelfRaidError                    ?.Invoke(this, e);
        client.OnNoPermissionError                  += (s, e) => OnNoPermissionError                ?.Invoke(this, e);
        client.OnRaidedChannelIsMatureAudience      += (s, e) => OnRaidedChannelIsMatureAudience    ?.Invoke(this, e);
        client.OnRitualNewChatter                   += (s, e) => OnRitualNewChatter                 ?.Invoke(this, e);
        client.OnHostingStopped                     += (s, e) => OnHostingStopped                   ?.Invoke(this, e);
        client.OnHostingStarted                     += (s, e) => OnHostingStarted                   ?.Invoke(this, e);
        client.OnUserLeft                           += (s, e) => OnUserLeft                         ?.Invoke(this, e);
        client.OnExistingUsersDetected              += (s, e) => OnExistingUsersDetected            ?.Invoke(this, e);
        client.OnLog                                += (s, e) => OnLog                              ?.Invoke(this, e);
        client.OnConnected                          += (s, e) => OnConnected                        ?.Invoke(this, e);
        client.OnJoinedChannel                      += (s, e) => OnJoinedChannel                    ?.Invoke(this, e);
        client.OnIncorrectLogin                     += (s, e) => OnIncorrectLogin                   ?.Invoke(this, e);
        client.OnChannelStateChanged                += (s, e) => OnChannelStateChanged              ?.Invoke(this, e);
        client.OnUserStateChanged                   += (s, e) => OnUserStateChanged                 ?.Invoke(this, e);
        client.OnMessageReceived                    += (s, e) => OnMessageReceived                  ?.Invoke(this, e);
        client.OnWhisperReceived                    += (s, e) => OnWhisperReceived                  ?.Invoke(this, e);
        client.OnFailureToReceiveJoinConfirmation   += (s, e) => OnFailureToReceiveJoinConfirmation ?.Invoke(this, e);
        client.OnMessageSent                        += (s, e) => OnMessageSent                      ?.Invoke(this, e);
        client.OnChatCommandReceived                += (s, e) => OnChatCommandReceived              ?.Invoke(this, e);
        client.OnWhisperCommandReceived             += (s, e) => OnWhisperCommandReceived           ?.Invoke(this, e);
        client.OnUserJoined                         += (s, e) => OnUserJoined                       ?.Invoke(this, e);
        client.OnModeratorJoined                    += (s, e) => OnModeratorJoined                  ?.Invoke(this, e);
        client.OnModeratorLeft                      += (s, e) => OnModeratorLeft                    ?.Invoke(this, e);
        client.OnNewSubscriber                      += (s, e) => OnNewSubscriber                    ?.Invoke(this, e);
        client.OnReSubscriber                       += (s, e) => OnReSubscriber                     ?.Invoke(this, e);
        client.OnHostLeft                           += (s, e) => OnHostLeft                         ?.Invoke(this, e);
        client.OnWhisperSent                        += (s, e) => OnWhisperSent                      ?.Invoke(this, e);
        client.OnUnaccountedFor                     += (s, e) => OnUnaccountedFor                   ?.Invoke(this, e);

        // Log client activity
        client.OnLog += (s, e) => Debug.Log(e.Data);
    }

    public void ConnectTo(string channelToJoin)
    {
        if (!client.IsInitialized)
        {
            client.Initialize(credentials, channelToJoin);
            client.OnJoinedChannel += (s, e) => SendMessage("Hello Twitch chat !");
        }

        if (!client.IsConnected)
            client.Connect();
        else
        {
            client.Initialize(credentials, channelToJoin);
            client.Reconnect();
        }
    }

    public void SendMessage(string message, bool dryRun = false)
    {
        if (client.IsInitialized && client.IsConnected)
            client.SendMessage(client.JoinedChannels[0], "MrDestructoid  " + message, dryRun);
        else
            Debug.Log("No channel joined, sending message failed");
    }

    public bool IsConnected
    {
        get
        {
            return client != null && client.IsInitialized && client.IsConnected;
        }
    }
}
