using TwitchLib.Client.Events;

namespace DancingICE.RythmGame.TwitchMode
{
    public class TwitchScoring : Scoring
    {

        private void Start()
        {
            TwitchClient.Instance.OnMessageReceived += IncreaseScore;
        }

        private void IncreaseScore(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message == SettingsManager.Twitch.AudienceTimeCommand)
            {
                multiplier += 0.1f;
            }
        }
    }
}