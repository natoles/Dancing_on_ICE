using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Api.Models.Helix.Streams.GetStreams;
using UnityEngine;
using Twitch = TwitchLib.Unity;

public class TwitchAPI : MonoBehaviour
{
    private Twitch.Api api;

    private void Start()
    {
        api = new Twitch.Api();
        api.Settings.AccessToken = AuthTokens.BOT_ACCESS_TOKEN;
        api.Settings.ClientId = AuthTokens.CLIENT_ID;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(GetViewersCount());
        }
    }

    private IEnumerator GetViewersCount()
    {
        GetStreamsResponse obj = null;
        yield return
            api.InvokeAsync(
                api.Streams.helix.GetStreamsAsync(userLogins: new List<string> { TwitchClient.Instance().JoinedChannels[0].Channel }),
                (response) => obj = response
                );

        if (obj.Streams.Length == 0)
            Debug.Log("User not live");
        else
            Debug.Log($"{obj.Streams[0].ViewerCount} viewers on stream");
    }
}
