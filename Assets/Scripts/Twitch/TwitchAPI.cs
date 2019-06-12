using Twitch = TwitchLib.Unity;

public class TwitchAPI : Twitch.Api
{
    public static TwitchAPI instance = null;
    
    public static TwitchAPI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TwitchAPI();
                instance.Settings.AccessToken = AuthTokens.BOT_ACCESS_TOKEN;
                instance.Settings.ClientId = AuthTokens.CLIENT_ID;
            }
            return instance;
        }
    }

    private TwitchAPI() { }
}
