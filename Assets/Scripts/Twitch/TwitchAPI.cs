using Twitch = TwitchLib.Unity;

public class TwitchAPI : Twitch.Api
{
    public static readonly TwitchAPI Instance = new TwitchAPI();

    static TwitchAPI()
    {
        Instance.Settings.AccessToken = AuthTokens.BOT_ACCESS_TOKEN;
        Instance.Settings.ClientId = AuthTokens.CLIENT_ID;
    }

    private TwitchAPI() { }
}
