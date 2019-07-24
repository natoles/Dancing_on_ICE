using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMusicScreen : MonoBehaviour
{
    [SerializeField]
    private Text TextComponent = null;

    [SerializeField]
    private Image LoadingRing = null;

    [SerializeField]
    private float ringAngularSpeed = 1f;
    
    public string Text
    {
        get
        {
            return TextComponent?.text;
        }
        set
        {
            if (TextComponent != null)
                TextComponent.text = value;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        LoadingRing.rectTransform.Rotate(-((Time.deltaTime * ringAngularSpeed) % 360) * Vector3.forward);
    }
}
