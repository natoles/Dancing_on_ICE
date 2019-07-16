using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager>
{
    [SerializeField]
    private float fadeDuration = 0.3f;

    [SerializeField]
    private float displayDuration = 2f;

    private NotificationPanel notificationPanelPrefab = null;
    private LinkedList<NotificationPanel> notifications = new LinkedList<NotificationPanel>();
    
    private Vector2 spawnPosition;

    private void Awake()
    {
        notificationPanelPrefab = Resources.Load<NotificationPanel>("Prefabs/NotificationPanel");
        spawnPosition = notificationPanelPrefab.rectTransform.anchoredPosition;
    }

    public void PushNotification(string text, Color textColor, Color bgColor)
    {   
        NotificationPanel notif = Instantiate(notificationPanelPrefab, transform);
        notif.name = "NotificationPanel";
        notif.textComponent.text = text;
        notif.textComponent.color = textColor;
        notif.color = bgColor;
        notif.startTime = Time.time;
        notif.stopTime = notif.startTime + 2*fadeDuration + displayDuration;
        notifications.AddFirst(notif);
    }

    private void Update()
    {
        Vector2 previous = Vector2.zero;
        LinkedListNode<NotificationPanel> lastAlive = null;
        bool needRemove = false;
        int i = 0;
        float cumulatedHeight = 0f;
        float previousHeight = 0f;
        for (LinkedListNode<NotificationPanel> it = notifications.First; it != null; it = it.Next)
        {
            NotificationPanel notif = it.Value;
            cumulatedHeight += notif.rectTransform.rect.height;
            if (Time.time <= notif.startTime + fadeDuration)
            {
                notif.rectTransform.anchoredPosition = Vector2.Lerp(spawnPosition, cumulatedHeight * Vector2.up - spawnPosition, (Time.time - notif.startTime) / fadeDuration);
                notif.SetAlpha((Time.time - notif.startTime) / fadeDuration);
            }
            else
            {
                notif.rectTransform.anchoredPosition = (previousHeight + notif.rectTransform.rect.height) * Vector2.up - spawnPosition;
                if (Time.time < notif.startTime + fadeDuration + displayDuration) { }
                else if (Time.time <= notif.stopTime)
                {
                    notif.SetAlpha((notif.stopTime - Time.time) / fadeDuration);
                }
                else
                {
                    Destroy(notif.gameObject);
                    if (!needRemove)
                    {
                        lastAlive = it.Previous;
                        needRemove = true;
                    }
                }
            }
            previousHeight = notif.rectTransform.anchoredPosition.y ;
            ++i;
        }

        if (needRemove)
        {
            while (notifications.Last != lastAlive)
                notifications.RemoveLast();
        }
    }
}
