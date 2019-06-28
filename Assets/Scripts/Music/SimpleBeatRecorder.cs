using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

public class SimpleBeatRecorder : MonoBehaviour
{
    public Button recordButton = null;
    public Button playButton = null;
    public Button loadButton = null;
    public Button saveButton = null;
    public AudioSource audioPlayer = null;

    public RectTransform beat1 = null;
    public RectTransform beat2 = null;

    private float minimalDuration = 0.2f;

    private Beatmap bm = null;
    private BeatTimestamp ts1 = null;
    private BeatTimestamp ts2 = null;

    private int mode = 0;
    private int i1 = 0;
    private int i2 = 0;
    
    private void Start()
    {
        recordButton.onClick.AddListener(StartRecording);
        playButton.onClick.AddListener(PlayRecording);
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
        mode = 1;
        audioPlayer.Stop();
        audioPlayer.PlayDelayed(1);
    }
    
    private void PlayRecording()
    {
        if (bm == null)
            return;
        mode = 2;
        i1 = 0;
        i2 = 0;
        audioPlayer.Stop();
        audioPlayer.PlayDelayed(1);
    }

    private void LoadBeatmap()
    {
        string path = BeatmapLoader.SelectBeatmapFile();
        if (path != null)
        {
            BeatmapContainer bmc = BeatmapLoader.LoadBeatmapFile(path);
            AudioClip audio = BeatmapLoader.LoadBeatmapAudio(bmc);
            audioPlayer.clip = audio;
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

        if (!audioPlayer.isPlaying)
            mode = 0;
        else
        {
            if (mode == 1)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (ts1 == null)
                    {
                        ts1 = new BeatTimestamp
                        {
                            type = BeatType.Simple,
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
                    bm.Pool1.Add(ts1);
                    ts1 = null;
                }

                if (Input.GetKey(KeyCode.I))
                {
                    if (ts2 == null)
                    {
                        ts2 = new BeatTimestamp
                        {
                            type = BeatType.Simple,
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
                    bm.Pool2.Add(ts2);
                    ts2 = null;
                }
            }
            else if (mode == 2)
            {
                if (i1 < bm.Pool1.Count)
                {
                    BeatTimestamp bts1 = bm.Pool1[i1];
                    if (musicTime >= bts1.time)
                    {
                        float duration = bts1.type == BeatType.Simple ? minimalDuration : bts1.duration;
                        if (musicTime <= bts1.time + duration)
                        {
                            beat1.localScale = Vector3.one * 1.2f;
                        }
                        else
                        {
                            beat1.localScale = Vector3.one;
                            i1++;
                        }
                    }
                }

                if (i2 < bm.Pool2.Count)
                {
                    BeatTimestamp bts2 = bm.Pool2[i2];
                    if (musicTime >= bts2.time)
                    {
                        float duration = bts2.type == BeatType.Simple ? 0.05f : bts2.duration;
                        if (musicTime <= bts2.time + duration)
                        {
                            beat2.localScale = Vector3.one * 1.2f;
                        }
                        else
                        {
                            beat2.localScale = Vector3.one;
                            i2++;
                        }
                    }
                }
            }
        }
    }
}
