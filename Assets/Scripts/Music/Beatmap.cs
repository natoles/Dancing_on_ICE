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
public class Beatmap
{
    public string AudioFile = null;
    public List<BeatTimestamp> Pool1 = new List<BeatTimestamp>();
    public List<BeatTimestamp> Pool2 = new List<BeatTimestamp>();
}

public class BeatmapContainer
{
    public AudioClip audio;
    public Beatmap bm;
}