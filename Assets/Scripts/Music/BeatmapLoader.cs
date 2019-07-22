using System.IO;
using System.Linq;
using UnityEngine;
using Crosstales.FB;
using NAudio.Wave;

static class BeatmapLoader
{
    #region File Formats

    public static string BeatmapFileFormat { get { return ".icebm"; } }
    public static string[] SupportedAudioFormats { get { return new string[] { ".mp3", ".wav" }; } }

    #endregion

    #region Utilities

    private static ExtensionFilter AudioExtensionFilter { get { return new ExtensionFilter("Supported Audio Files", SupportedAudioFormats); } }
    private static ExtensionFilter BeatmapExtensionFilter { get { return new ExtensionFilter("ICE Beatmap", BeatmapFileFormat); } }

    private static string GetValidPath(System.Environment.SpecialFolder wanted = System.Environment.SpecialFolder.MyDocuments)
    {
        string baseFolder = null;
        baseFolder = System.Environment.GetFolderPath(wanted);
        if (baseFolder == "")
        {
            baseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            if (baseFolder == "")
                baseFolder = Application.persistentDataPath;
        }
        return baseFolder;
    }

    #endregion

    #region File Selection

    public static string SelectAudioFile()
    {
        string defaultPath = GetValidPath(System.Environment.SpecialFolder.MyMusic);
        return FileBrowser.OpenSingleFile("Open audio file", defaultPath, AudioExtensionFilter);
    }

    // Not async, because the feature is disabled in original library
    public static void SelectAudioFileAsync(System.Action<string[]> callback)
    {
        string defaultPath = GetValidPath(System.Environment.SpecialFolder.MyMusic);
        FileBrowser.OpenFilesAsync(callback, "Open audio file", defaultPath, multiselect: false, AudioExtensionFilter);
    }

    public static string SelectBeatmapFile()
    {
        string defaultPath = GetValidPath();
        return FileBrowser.OpenSingleFile("Open beatmap", defaultPath, BeatmapExtensionFilter);
    }

    // Not async, because the feature is disabled in original library
    public static void SelectBeatmapFileAsync(System.Action<string[]> callback)
    {
        string defaultPath = GetValidPath();
        FileBrowser.OpenFilesAsync(callback, "Open beatmap", defaultPath, multiselect: false, BeatmapExtensionFilter);
    }

    #endregion

    #region Beatmap Loading

    public static BeatmapContainer LoadBeatmapFile(string path)
    {
        if (Path.GetExtension(path) != BeatmapFileFormat)
            return null;

        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();

        Beatmap bm = JsonUtility.FromJson<Beatmap>(json);
        if (bm == null)
            return null;

        return new BeatmapContainer { sourceFile = Path.GetFileName(path), directory = Path.GetDirectoryName(path), bm = bm };
    }

    public static BeatmapContainer CreateBeatmapFromAudio(string path)
    {
        if (!SupportedAudioFormats.Contains<string>(Path.GetExtension(path)))
            return null;
        
        return new BeatmapContainer
        {
            sourceFile = Path.GetFileName(path),
            directory = Path.GetDirectoryName(path),
            bm = new Beatmap
            {
                AudioFile = Path.GetFileName(path)
            }
        };
    }

    #endregion

    #region Audio Loading

    public static AudioClip CreateAudioClipFromData(AudioClipData cd)
    {
        AudioClip audio = AudioClip.Create(cd.name, cd.lengthSamples, cd.channels, cd.frequency, cd.stream);
        audio.SetData(cd.data, cd.offsetSamples);
        return audio;
    }

    public static AudioClipData LoadBeatmapAudio(BeatmapContainer bmc)
    {
        if (bmc == null || bmc.directory == null || bmc.bm.AudioFile == null)
            return null;

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
        if (reader == null)
            return null;

        int size = (int) (reader.Length / sizeof(float));
        float[] audioData = new float[size];
        reader.Read(audioData, 0, size);

        return new AudioClipData (basename, size / reader.WaveFormat.Channels, reader.WaveFormat.Channels, reader.WaveFormat.SampleRate, false, audioData, 0);
    }

    #endregion
}