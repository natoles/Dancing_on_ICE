using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

public class SimpleBeatRecorder : MonoBehaviour
{
    public Button recordButton = null;
    public Button loadButton = null;
    public Button saveButton = null;
    public AudioSource audioPlayer = null;

    public RectTransform beat1 = null;
    public RectTransform beat2 = null;

    private float minimalDuration = 0.2f;

    private Beatmap bm = null;
    private BeatTimestamp ts1 = null;
    private BeatTimestamp ts2 = null;
    
    private void Start()
    {
        recordButton.onClick.AddListener(StartRecording);
        loadButton.onClick.AddListener(LoadBeatmap);
        saveButton.onClick.AddListener(SaveBeatmap);
    }

    private void StartRecording()
    {
        if (audioPlayer.clip == null)
        {
            BeatmapLoader.SelectAudioFile();
        }

        bm = new Beatmap();
        audioPlayer.Stop();
        audioPlayer.PlayDelayed(1);
    }

    private void LoadBeatmap()
    {
        string path = BeatmapLoader.SelectBeatmapFile();
        if (path != null)
        {
            BeatmapContainer bmc = BeatmapLoader.LoadBeatmapFile(path);
            AudioClipData cd = BeatmapLoader.LoadBeatmapAudio(bmc);
            audioPlayer.clip = BeatmapLoader.CreateAudioClipFromData(cd);
            bm = bmc.bm;
        }
    }

    private void SaveBeatmap()
    {
        string path = FileBrowser.SaveFile(audioPlayer.clip.name, "icebm");
        if (path != null)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(JsonUtility.ToJson(bm));
            writer.Close();
        }
    }

    private void Update()
    {
        float musicTime = audioPlayer.time;

        if (audioPlayer.isPlaying)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (ts1 == null)
                {
                    ts1 = new BeatTimestamp
                    {
                        type = BeatType.Simple,
                        joint = Windows.Kinect.JointType.HandLeft,
                        time = musicTime,
                        duration = 0
                    };
                    beat1.localScale = Vector3.one * 1.2f;
                }
            }
            else if (ts1 != null)
            {
                if (musicTime - ts1.time > minimalDuration)
                {
                    ts1.type = BeatType.Slider;
                    ts1.duration = musicTime - ts1.time;
                }

                beat1.localScale = Vector3.one;
                bm.Pool.Add(ts1);
                ts1 = null;
            }

            if (Input.GetKey(KeyCode.I))
            {
                if (ts2 == null)
                {
                    ts2 = new BeatTimestamp
                    {
                        type = BeatType.Simple,
                        joint = Windows.Kinect.JointType.HandRight,
                        time = musicTime,
                        duration = 0
                    };
                    beat2.localScale = Vector3.one * 1.2f;
                }
            }
            else if (ts2 != null)
            {
                if (musicTime - ts2.time > minimalDuration)
                {
                    ts2.type = BeatType.Slider;
                    ts2.duration = musicTime - ts2.time;
                }

                beat2.localScale = Vector3.one;
                bm.Pool.Add(ts2);
                ts2 = null;
            }
        }
    }
}
