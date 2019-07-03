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
    
    [System.NonSerialized]
    public BeatmapContainer BeatmapContainer = null;

    private int id = 0;

    [System.NonSerialized]
    public SongScrollView ScrollView = null;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        ScrollView.ScrollToSong(id);
        TwitchRythmController.beatmapToLoad = BeatmapContainer;
    }

    public void SetSong(int id, string icebmFile)
    {
        if (System.IO.Path.GetExtension(icebmFile) != ".icebm")
            return;

        this.id = id;
        BeatmapContainer = BeatmapLoader.LoadBeatmapFile(icebmFile);
        Thumbnail.sprite = null;
        SongName.text = BeatmapContainer.bm.Metadata.SongName;
        Artist.text = BeatmapContainer.bm.Metadata.Artist;
        Difficulty.text = Math.Round(BeatmapContainer.bm.Metadata.Difficulty, 1).ToString();
        Difficulty.color = Color.Lerp(Color.green, Color.red, BeatmapContainer.bm.Metadata.Difficulty / 5f);
        Duration.text = TimeSpan.FromSeconds(BeatmapContainer.bm.Metadata.Duration).ToString(@"mm\:ss");
    }
}
