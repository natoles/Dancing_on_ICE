using System;
using System.Linq;
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
            SongName.text = tfile.Tag.Title != null ? tfile.Tag.Title : IO.Path.GetFileNameWithoutExtension(sourceFile);
            Artist.text = tfile.Tag.FirstPerformer;
            //Difficulty.text = Math.Round(BeatmapContainer.bm.Metadata.Difficulty, 1).ToString();
            //Difficulty.color = Color.Lerp(Color.green, Color.red, BeatmapContainer.bm.Metadata.Difficulty / 5f);
            Duration.text = tfile.Properties.Duration.ToString(@"mm\:ss");
        }
    }
}
