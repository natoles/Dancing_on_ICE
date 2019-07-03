using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Button that can be clicked only when TwitchClient is connected
/// </summary>
public class TwitchOnlyButton : Button
{
    private void Update()
    {
        if (Application.isPlaying)
        {
            if (TwitchClient.Instance.IsConnected)
                interactable = true;
            else
                interactable = false;
        }
    }
}
