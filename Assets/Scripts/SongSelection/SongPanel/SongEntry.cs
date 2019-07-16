using System;
using IO = System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SongEntry : Button
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
        TwitchRythmController.BeatmapToLoad = BeatmapContainer;
    }

    public void SetSong(int id, string icebmFile)
    {
        if (System.IO.Path.GetExtension(icebmFile) != ".icebm")
            return;

        this.id = id;
        BeatmapContainer = BeatmapLoader.LoadBeatmapFile(icebmFile);
        Thumbnail.sprite = null;
        TagLib.File tfile = TagLib.File.Create(IO.Path.Combine(BeatmapContainer.directory, BeatmapContainer.bm.AudioFile));
        SongName.text = tfile.Tag.Title != null ? tfile.Tag.Title : IO.Path.GetFileNameWithoutExtension(BeatmapContainer.sourceFile);
        Artist.text = tfile.Tag.FirstPerformer;
        Difficulty.text = Math.Round(BeatmapContainer.bm.Metadata.Difficulty, 1).ToString();
        Difficulty.color = Color.Lerp(Color.green, Color.red, BeatmapContainer.bm.Metadata.Difficulty / 5f);
        Duration.text = tfile.Properties.Duration.ToString(@"mm\:ss");
    }
}
