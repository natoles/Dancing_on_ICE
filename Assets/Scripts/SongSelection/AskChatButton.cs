using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AskChatButton : Button, IPointerClickHandler
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

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Debug.Log("Ask chat");
    }
}
