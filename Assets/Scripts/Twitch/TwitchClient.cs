using System;
using Twitch = TwitchLib.Unity;
using TwitchLib.Client.Models;

public class TwitchClient : Twitch.Client
{
    private static readonly TwitchClient instance = new TwitchClient();
    private static readonly ConnectionCredentials credentials = new ConnectionCredentials(AuthTokens.BOT_NAME, AuthTokens.BOT_ACCESS_TOKEN);

    private TwitchClient() {}
    
    public static TwitchClient Instance()
    {
        return instance;
    }

    public void ConnectTo(string channelToJoin)
    {
        base.Initialize(credentials, channelToJoin);
        base.Connect();
    }

    public void SendMessage(string message, bool dryRun = false)
    {
        if (base.JoinedChannels.Count > 0)
            base.SendMessage(base.JoinedChannels[0], message, dryRun);
        else
            Console.WriteLine("No channel joined, sending message failed");
    }
}
