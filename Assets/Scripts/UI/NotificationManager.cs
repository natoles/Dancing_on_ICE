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

    private float notifHeight;
    private Vector2 baseMove;
    private Vector2 spawnPosition;

    private void Awake()
    {
        notificationPanelPrefab = Resources.Load<NotificationPanel>("Prefabs/NotificationPanel");
        notifHeight = notificationPanelPrefab.rectTransform.sizeDelta.y;
        baseMove = new Vector2(0, notifHeight);
        spawnPosition = notificationPanelPrefab.rectTransform.anchoredPosition;
    }

    public void PushNotification(string text, Color textColor, Color bgColor)
    {
        Debug.Log(text);
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            PushNotification("Debug " + (int)Random.Range(0, 100), Color.black, Color.white);
        }

        Vector2 previous = Vector2.zero;
        LinkedListNode<NotificationPanel> lastAlive = null;
        bool needRemove = false;
        int i = 0;
        for (LinkedListNode<NotificationPanel> it = notifications.First; it != null; it = it.Next)
        {
            NotificationPanel notif = it.Value;
            if (Time.time <= notif.startTime + fadeDuration)
            {
                notif.rectTransform.anchoredPosition = Vector2.Lerp(spawnPosition, i * baseMove - spawnPosition, (Time.time - notif.startTime) / fadeDuration);
                notif.SetAlpha((Time.time - notif.startTime) / fadeDuration);
            }
            else
            {
                notif.rectTransform.position = previous;
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
            previous = notif.rectTransform.anchoredPosition + baseMove;
            ++i;
        }

        if (needRemove)
        {
            while (notifications.Last != lastAlive)
                notifications.RemoveLast();
        }
    }
}
