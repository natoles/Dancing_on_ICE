using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SongEntry : Button, IPointerClickHandler
{
    [SerializeField]
    private Image Thumbnail;

    [SerializeField]
    private Text SongName;

    [SerializeField]
    private Text Artist;

    [SerializeField]
    private Text Difficulty;

    [SerializeField]
    private Text Duration;

    private BeatmapContainer bmc = null;
    private int id = 0;

    private bool selected = false;

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!selected)
        {
            selected = true;
            base.OnPointerClick(pointerEventData);
        }
        else
        {
            PlaySong();
        }
    }

    public void SetSong(int id, string icebmFile)
    {
        if (System.IO.Path.GetExtension(icebmFile) != ".icebm")
            return;

        this.id = id;
        bmc = BeatmapLoader.LoadBeatmapFile(icebmFile);
        SongName.text = bmc.bm.Metadata.SongName;
        Artist.text = bmc.bm.Metadata.Artist;
        Difficulty.text = Math.Round(bmc.bm.Metadata.Difficulty, 1).ToString();
        Difficulty.color = Color.Lerp(Color.green, Color.red, bmc.bm.Metadata.Difficulty / 5f);
        Duration.text = TimeSpan.FromSeconds(bmc.bm.Metadata.Duration).ToString(@"mm\:ss");
    }

    public void PlaySong()
    {
        TwitchRythmController.beatmapToLoad = bmc;
        SceneManager.LoadScene("TwitchMode");
    }
}
