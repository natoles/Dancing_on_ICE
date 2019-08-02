using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : Image
{
    public Text textComponent { get; private set; } = null;

    public float startTime = float.MaxValue - 1;
    public float stopTime = float.MaxValue;

    protected override void Awake()
    {
        textComponent = GetComponentInChildren<Text>();
    }

    public void SetAlpha(float a)
    {
        color = new Color(color.r, color.g, color.b, a);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, a);
    }
}
