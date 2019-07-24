using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteEntry : Image
{
    [SerializeField]
    private Text IDText = null;

    [SerializeField]
    private Text SongNameText = null;

    private SongEntry song;
    private float baseFill = 0f;
    private float tmpFill = 0f;

    public string Id
    {
        get
        {
            return IDText?.text;
        }
        set
        {
            if (IDText != null)
                IDText.text = value;
        }
    }

    public SongEntry Song
    {
        get
        {
            return song;
        }
        set
        {
            song = value;
            if (SongNameText != null)
                SongNameText.text = value.SongName;
        }
    }
    
    public float Percentage
    {
        get
        {
            return percentage;
        }
        set
        {
            percentage = Mathf.Clamp01(value);
        }
    }

    private float percentage = 0;

    private readonly float updateDelay = 0.1f;

    protected override void Start()
    {
        Canvas.ForceUpdateCanvases();
        baseFill = 0.1f * rectTransform.rect.height / rectTransform.rect.width;
        fillAmount = baseFill;
        tmpFill = baseFill;
    }

    private void Update()
    {
        tmpFill = Mathf.Lerp(tmpFill, Mathf.Clamp01(baseFill + percentage), updateDelay);
        fillAmount = tmpFill;
    }
}
