using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class AudienceTime : MonoBehaviour
{
    public Text noticeText = null;
    public Text multiplierText = null;
    public Image timerImage = null;

    private float startTime = float.MaxValue;
    public float duration = 0f;

    private bool started = false;
    private float multiplier = 1f;

    private TwitchClient client = null;

    private void Start()
    {
        client = TwitchClient.Instance();
        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !started)
        {
            startTime = Time.time;
        }

        if (!started && Time.time >= startTime && Time.time <= startTime + duration)
        {
            Debug.Log("Starting Audience Time");
            Show();
            client.SendMessage($"Audience time ! Type {ChatCommands.AUDIENCE_TIME_CMD.ToUpperInvariant()} in chat to increase the multiplier !");
            client.OnMessageReceived += AudienceTime_Handler;
            started = true;
        }
        else if (started && Time.time > startTime + duration)
        {
            Debug.Log("Ending Audience Time");
            client.OnMessageReceived -= AudienceTime_Handler;
            Hide();
            started = false;
        }

        if (started)
        {
            GetComponent<Text>().text = $"Time left:\n{TimeSpan.FromSeconds(startTime + duration - Time.time).ToString(@"mm\:ss\:ff")}";
            timerImage.fillAmount = (startTime + duration - Time.time) / duration;
            AddToMultiplier(-0.01f * Time.deltaTime);
        }
    }

    private void AudienceTime_Handler(object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message.ToLowerInvariant() == ChatCommands.AUDIENCE_TIME_CMD)
        {

            AddToMultiplier(0.1f);
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
}
