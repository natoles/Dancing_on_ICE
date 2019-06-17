using System;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : Image
{
    public Text textComponent { get; private set; } = null;

    private float startTime = float.MaxValue - 1;
    private float stopTime = float.MaxValue;

    [SerializeField]
    private float transitionTime = 0.3f;
    
    [SerializeField]
    private float displayTime = 2.5f;
    
    [SerializeField]
    private Vector3 initialPosition = new Vector3(0, 0, 0);

    [SerializeField]
    private Vector3 finalPosition = new Vector3(0, 40, 0);

    protected override void Awake()
    {
        textComponent = GetComponentInChildren<Text>();
    }
    
    public void Trigger()
    {
        startTime = Time.time;
        stopTime = startTime + 2 * transitionTime + displayTime;
    }

    private void Update()
    {
        float coeff = 0;
        if (Time.time < startTime)
        {
            return;
        }
        else
        {
            if (Time.time <= startTime + transitionTime + displayTime / 2)
            {
                coeff = (Time.time - startTime) / transitionTime;
            }
            else if (Time.time <= stopTime)
            {
                coeff = (stopTime - Time.time) / transitionTime;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        rectTransform.anchoredPosition = Vector3.Lerp(initialPosition, finalPosition, coeff);
    }
}
