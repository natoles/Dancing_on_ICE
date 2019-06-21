using System;
using System.Collections.Generic;

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
public class BeatMap
{
    public List<BeatTimestamp> Pool1 = new List<BeatTimestamp>();
    public List<BeatTimestamp> Pool2 = new List<BeatTimestamp>();
}