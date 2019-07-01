using System;
using System.Collections.Generic;
using UnityEngine;

public enum BeatType
{
    Simple,
    Slider
}

[Serializable]
public class BeatTimestamp
{
    public BeatType type = BeatType.Simple;
    public float time = -1;
    public float duration = 0;
}

[Serializable]
public class BeatmapInfo
{
    public string SongName = null;
    public string Artist = null;
    public float Duration = 0;
    public float Difficulty = 0;
}

[Serializable]
public class Beatmap
{
    public BeatmapInfo Metadata = new BeatmapInfo();
    public string AudioFile = null;
    public List<BeatTimestamp> Pool1 = new List<BeatTimestamp>();
    public List<BeatTimestamp> Pool2 = new List<BeatTimestamp>();
}

public class BeatmapContainer
{
    public string sourceFile;
    public string directory;
    public Beatmap bm;
}

public class AudioClipData
{
    public string name;
    public int lengthSamples;
    public int channels;
    public int frequency;
    public bool stream;
    public float[] data;
    public int offsetSamples;

    public AudioClipData(string name, int lengthSamples, int channels, int frequency, bool stream, float[] data, int offsetSamples)
    {
        this.name = name;
        this.lengthSamples = lengthSamples;
        this.channels = channels;
        this.frequency = frequency;
        this.stream = stream;
        this.data = data;
        this.offsetSamples = offsetSamples;
    }
}