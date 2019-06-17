using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class AudienceTimeManager : MonoBehaviour
{
    public Text noticeText = null;
    public Text multiplierText = null;
    public Image timerImage = null;

    private float startTime = float.MaxValue;
    public float duration = 0f;

    private bool started = false;
    private float multiplier = 1f;

    public Image msgTimeoutImage = null;
    private float lastMessageTime = float.MaxValue;
    public float maxDelay = 0f;

    private bool quitting = false;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (TwitchClient.Instance.IsConnected)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !started)
            {
                startTime = Time.time;
                lastMessageTime = startTime;
                Show();
                TwitchClient.Instance.SendMessage($"Audience time ! Type {SettingsManager.Instance.twitch[SettingTyp.AudTimCmd].value.ToUpperInvariant()} in chat to increase the multiplier !");
                TwitchClient.Instance.OnMessageReceived += AudienceTime_Handler;
                Debug.Log("Starting Audience Time");
                started = true;
            }
            else if (started && Time.time > startTime + duration)
            {
                Debug.Log("Ending Audience Time");
                TwitchClient.Instance.OnMessageReceived -= AudienceTime_Handler;
                TwitchClient.Instance.SendMessage("Audience time is now finished ! Thanks for your participation !");
                Hide();
                started = false;
            }

            if (started)
            {
                GetComponent<Text>().text = $"Time left:\n{TimeSpan.FromSeconds(startTime + duration - Time.time).ToString(@"mm\:ss\:ff")}";
                timerImage.fillAmount = (startTime + duration - Time.time) / duration;
                msgTimeoutImage.fillAmount = (Time.time - lastMessageTime) / maxDelay;
                if (Time.time > lastMessageTime + maxDelay)
                {
                    Debug.Log("TIMEOUT");
                    AddToMultiplier(-0.5f);
                    lastMessageTime = Time.time;
                }
            }
        }
    }

    private void AudienceTime_Handler(object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message.ToLowerInvariant() == SettingsManager.Instance.twitch[SettingTyp.AudTimCmd].value)
        {
            AddToMultiplier(0.1f);
            lastMessageTime = Time.time;
        }
    }

    private void AddToMultiplier(float value)
    {
        multiplier += value;
        if (multiplier < 1f)
            multiplier = 1f;
        else if (multiplier > 3f)
            multiplier = 3f;
        multiplierText.text = $"Multiplier:\nx{System.Math.Round(multiplier,2)}";
    }

    private void Show()
    {
        noticeText.canvasRenderer.SetAlpha(1f);
        multiplierText.canvasRenderer.SetAlpha(1f);
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(1f);
        timerImage.canvasRenderer.SetAlpha(1f);
    }

    private void Hide()
    {
        noticeText.canvasRenderer.SetAlpha(0f);
        multiplierText.canvasRenderer.SetAlpha(0f);
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(0f);
        timerImage.canvasRenderer.SetAlpha(0f);
    }

    private void OnApplicationQuit()
    {
        quitting = true;
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && !quitting)
        {
            TwitchClient.Instance.OnMessageReceived -= AudienceTime_Handler;
        }
    }
}
