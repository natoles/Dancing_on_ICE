using System;
using System.Linq;
using IO = System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DancingICE.RythmGame;

public class SongEntry : Button
{
    [SerializeField]
    private Image Thumbnail = null;

    [SerializeField]
    private Text SongNameText = null;

    [SerializeField]
    private Text ArtistText = null;

    [SerializeField]
    private Text DifficultyText = null;

    [SerializeField]
    private Text DurationText = null;
    
    [System.NonSerialized]
    public BeatmapContainer BeatmapContainer = null;

    private int id = 0;

    [System.NonSerialized]
    public SongScrollView ScrollView = null;
    
    public string SongName
    {
        get
        {
            return SongNameText.text;
        }
        set
        {
            SongNameText.text = value;
        }
    }

    public string ArtistName
    {
        get
        {
            return ArtistText.text;
        }
        set
        {
            ArtistText.text = value;
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        ScrollView.ScrollToSong(id);
        RythmGameSettings.BeatmapToLoad = BeatmapContainer;
    }

    public void SetSong(int id, string path)
    {
        string extension = IO.Path.GetExtension(path);
        bool isAudio = BeatmapLoader.SupportedAudioFormats.Contains<string>(extension);
        bool isBeatmap = (extension == BeatmapLoader.BeatmapFileFormat);
        if (isAudio || isBeatmap)
        {
            string audioFile;
            string sourceFile;
            if (isAudio)
            {
                BeatmapContainer = BeatmapLoader.CreateBeatmapFromAudio(path);
                audioFile = sourceFile = path;
            }
            else
            {
                BeatmapContainer = BeatmapLoader.LoadBeatmapFile(path);
                audioFile = IO.Path.Combine(BeatmapContainer.directory, BeatmapContainer.bm.AudioFile);
                sourceFile = BeatmapContainer.sourceFile;
            }

            this.id = id;
            Thumbnail.sprite = null;
            TagLib.File tfile = TagLib.File.Create(audioFile);
            SongName = tfile.Tag.Title != null ? tfile.Tag.Title : IO.Path.GetFileNameWithoutExtension(sourceFile);
            ArtistName = tfile.Tag.FirstPerformer;
            DifficultyText.text = "N/A";
            //Difficulty.text = Math.Round(BeatmapContainer.bm.Metadata.Difficulty, 1).ToString();
            //Difficulty.color = Color.Lerp(Color.green, Color.red, BeatmapContainer.bm.Metadata.Difficulty / 5f);
            DurationText.text = tfile.Properties.Duration.ToString(@"mm\:ss");
        }
    }
}
