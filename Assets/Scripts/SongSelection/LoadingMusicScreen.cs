using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMusicScreen : MonoBehaviour
{
    [SerializeField]
    private Text SongName = null;

    [SerializeField]
    private Image LoadingRing = null;

    [SerializeField]
    private float ringAngularSpeed = 1f;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Display(string songName = null)
    {
        SongName.text = songName;
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
