using System.IO;
using UnityEngine;
using Crosstales.FB;
using NAudio.Wave;

static class BeatmapLoader
{
    public static string SelectAudioFile()
    {
        string baseFolder = null;
        baseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic);
        if (baseFolder == "")
        {
            baseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            if (baseFolder == "")
                baseFolder = Application.persistentDataPath;
        }

        return FileBrowser.OpenSingleFile("Open sound file", baseFolder, new ExtensionFilter("Supported Audio Files", "mp3", "wav", "ogg"));
    }

    public static string SelectBeatmapFile()
    {
        string baseFolder = null;
        baseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        if (baseFolder == "")
        {
            baseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            if (baseFolder == "")
                baseFolder = Application.persistentDataPath;
        }

        return FileBrowser.OpenSingleFile("Open beatmap", baseFolder, "icebm");
    }

    public static BeatmapContainer LoadBeatmapFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();

        Beatmap bm = JsonUtility.FromJson<Beatmap>(json);
        if (bm == null)
            return null;

        return new BeatmapContainer { sourceFile = Path.GetFileName(path), directory = Path.GetDirectoryName(path), bm = bm };
    }

    public static AudioClip CreateAudioClipFromData(AudioClipData cd)
    {
        AudioClip audio = AudioClip.Create(cd.name, cd.lengthSamples, cd.channels, cd.frequency, cd.stream);
        audio.SetData(cd.data, cd.offsetSamples);
        return audio;
    }

    public static AudioClipData LoadBeatmapAudio(BeatmapContainer bmc)
    {
        return LoadAudioFile(Path.Combine(bmc.directory, bmc.bm.AudioFile));
    }

    public static AudioClipData LoadAudioFile(string path)
    {
        string basename = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        AudioType type = extension == ".mp3" ? AudioType.MPEG : extension == ".wav" ? AudioType.WAV : extension == ".ogg" ? AudioType.OGGVORBIS : AudioType.UNKNOWN;

        if (type == AudioType.UNKNOWN)
        {
            NotificationManager.Instance.PushNotification(extension + " audio files are not supported", Color.white, Color.red);
            return null;
        }

        AudioFileReader reader = new AudioFileReader(path);
        int size = (int) (reader.Length / sizeof(float));
        float[] audioData = new float[size];
        reader.Read(audioData, 0, size);

        return new AudioClipData (basename, size / reader.WaveFormat.Channels, reader.WaveFormat.Channels, reader.WaveFormat.SampleRate, false, audioData, 0);
    }
}