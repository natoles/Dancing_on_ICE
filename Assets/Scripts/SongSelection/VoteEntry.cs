using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteEntry : Image
{
    [SerializeField]
    private Text ID = null;

    [SerializeField]
    private Text SongName = null;

    private float baseFill = 0f;
    private float tmpFill = 0f;

    [System.NonSerialized]
    public float percentage = 0;

    private float updateDelay = 0.1f;

    protected override void Start()
    {
        Canvas.ForceUpdateCanvases();
        baseFill = 0.1f * rectTransform.rect.height / rectTransform.rect.width;
        fillAmount = baseFill;
        tmpFill = baseFill;
    }

    private void Update()
    {
        if (Application.isPlaying)
            percentage = Mathf.Clamp01(percentage + Random.Range(-0.25f, 0.25f));
        tmpFill = Mathf.Lerp(tmpFill, baseFill + percentage, updateDelay);
        fillAmount = tmpFill;
    }

    public void SetSongName(string songName)
    {
        SongName.text = songName;
    }
}
