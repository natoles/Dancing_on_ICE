using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour
{
    private TwitchClient client;
    private Text chat = null;

    private void Start()
    {
        client = TwitchClient.Instance();
        chat = GetComponent<Text>();
        client.ConnectTo("twifus123");
        client.OnMessageReceived += Chat_OnMessageReceived;
    }

    private void Chat_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        chat.text += "\n" + e.ChatMessage.Username + ": " + e.ChatMessage.Message;
    }
    
}
