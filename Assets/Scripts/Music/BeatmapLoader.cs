using System;
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

    private static ExtensionFilter AudioExtensionFilter { get { return new ExtensionFilter("Supported Audio Files", SupportedAudioFormats.Select(str => str.TrimStart('.')).ToArray<string>()); } }
    private static ExtensionFilter BeatmapExtensionFilter { get { return new ExtensionFilter("ICE Beatmap", BeatmapFileFormat.TrimStart('.')); } }

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
        string extension = Path.GetExtension(path);
        if (extension != BeatmapFileFormat)
            throw new NotSupportedException($"Unsupported {extension} beatmap format (expected: {BeatmapFileFormat})");

        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();

        Beatmap bm = JsonUtility.FromJson<Beatmap>(json); // FIXME: Switch to Newtonsoft.Json
        if (bm == null)
            return null; // FIXME : throw appropriate exception

        return new BeatmapContainer { sourceFile = Path.GetFileName(path), directory = Path.GetDirectoryName(path), bm = bm };
    }

    public static BeatmapContainer CreateBeatmapFromAudio(string path)
    {
        string extension = Path.GetExtension(path);
        if (!SupportedAudioFormats.Contains<string>(extension))
            throw new NotSupportedException($"{extension} files are not supported");
        
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

    public static AudioClip CreateAudioClipFromData(AudioClipData clipData)
    {
        AudioClip audio = AudioClip.Create(clipData.name, clipData.lengthSamples, clipData.channels, clipData.frequency, clipData.stream);
        audio.SetData(clipData.data, clipData.offsetSamples);
        return audio;
    }

    public static AudioClipData LoadBeatmapAudio(BeatmapContainer bmc)
    {
        if (bmc == null)
            throw new ArgumentNullException(nameof(bmc));

        if (bmc.directory == null)
            throw new NullReferenceException();

        if (bmc.bm.AudioFile == null)
            throw new NullReferenceException();

        return LoadAudioFile(Path.Combine(bmc.directory, bmc.bm.AudioFile));
    }

    public static AudioClipData LoadAudioFile(string path)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));

        string basename = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        AudioType type = extension == ".mp3" ? AudioType.MPEG : extension == ".wav" ? AudioType.WAV : AudioType.UNKNOWN;

        if (type == AudioType.UNKNOWN)
            throw new NotSupportedException($"{extension} files are not supported");

        AudioFileReader reader = new AudioFileReader(path);

        int size = (int) (reader.Length / sizeof(float));
        float[] audioData = new float[size];
        reader.Read(audioData, 0, size);

        return new AudioClipData (basename, size / reader.WaveFormat.Channels, reader.WaveFormat.Channels, reader.WaveFormat.SampleRate, false, audioData, 0);
    }

    #endregion
}