using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongEntry : Button, IPointerClickHandler
{
    [SerializeField]
    private Image Thumbnail = null;

    [SerializeField]
    private Text SongName = null;

    [SerializeField]
    private Text Artist = null;

    [SerializeField]
    private Text Difficulty = null;

    [SerializeField]
    private Text Duration = null;

    private BeatmapContainer bmc = null;
    private int id = 0;

    public SongScrollView ScrollView = null;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        ScrollView.ScrollToSong(id);
        TwitchRythmController.beatmapToLoad = bmc;
    }

    public void SetSong(int id, string icebmFile)
    {
        if (System.IO.Path.GetExtension(icebmFile) != ".icebm")
            return;

        this.id = id;
        bmc = BeatmapLoader.LoadBeatmapFile(icebmFile);
        Thumbnail.sprite = null;
        SongName.text = bmc.bm.Metadata.SongName;
        Artist.text = bmc.bm.Metadata.Artist;
        Difficulty.text = Math.Round(bmc.bm.Metadata.Difficulty, 1).ToString();
        Difficulty.color = Color.Lerp(Color.green, Color.red, bmc.bm.Metadata.Difficulty / 5f);
        Duration.text = TimeSpan.FromSeconds(bmc.bm.Metadata.Duration).ToString(@"mm\:ss");
    }
}
