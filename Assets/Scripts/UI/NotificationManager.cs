using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager>
{
    public NotificationPanel notificationPanelPrefab;

    [SerializeField]
    private float fadeDuration = 0.3f;

    [SerializeField]
    private float displayDuration = 2f;

    [SerializeField]
    private float translationDuration = 0.3f;

    private List<NotificationPanel> notifications = new List<NotificationPanel>();

    public void PushNotification(string text, Color textColor, Color bgColor)
    {
        Debug.Log(text);
        NotificationPanel notif = Instantiate<NotificationPanel>(notificationPanelPrefab, transform);
        notif.name = "NotificationPanel";
        notif.textComponent.text = text;
        notif.textComponent.color = textColor;
        notif.color = bgColor;
        notif.startTime = Time.time;
        notif.stopTime = notif.startTime + 2*fadeDuration + displayDuration;
        notifications.Add(notif);
    }

    private void Update()
    {
        for (int i = 0; i < notifications.Count; ++i)
        {
            NotificationPanel notif = notifications[i];
            notif.rectTransform.position = new Vector2(0, (notifications.Count - i - 1) * notificationPanelPrefab.rectTransform.sizeDelta.y);
            if (Time.time <= notif.startTime + fadeDuration)
            {
                notif.SetAlpha((Time.time - notif.startTime) / fadeDuration);
            }
            else if (Time.time < notif.startTime + fadeDuration + displayDuration) {}
            else if (Time.time <= notif.stopTime)
            {
                notif.SetAlpha((notif.stopTime - Time.time) / fadeDuration);
            }
            else
            {
                Destroy(notif.gameObject);
                notifications[i] = null;
            }
        }
        notifications.RemoveAll(item => item == null);
    }
}
