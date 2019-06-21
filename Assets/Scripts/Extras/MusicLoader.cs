using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;
using NAudio.Wave;
using WaveFormRendererLib;

class MusicLoader : MonoBehaviour
{
    private AudioSource player = null;
    private Image image = null;
    public Image cursor = null;

    private void Start()
    {
        player = GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string path = SelectAudioFile();
            LoadAudioClip(path);
            RenderMusic(path);
        }

        if (player != null && player.isPlaying)
            cursor.rectTransform.anchoredPosition = new Vector2(player.time / player.clip.length * image.preferredWidth + image.rectTransform.rect.x, 0);
        else
            image.sprite = null;
    }

    private string SelectAudioFile()
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

    private void LoadAudioClip(string path)
    {
        string basename = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        AudioType type = extension == ".mp3" ? AudioType.MPEG : extension == ".wav" ? AudioType.WAV : extension == ".ogg" ? AudioType.OGGVORBIS : AudioType.UNKNOWN;


        AudioFileReader reader = new AudioFileReader(path);
        int size = (int) (reader.Length / sizeof(float));
        float[] audioData = new float[size];
        reader.Read(audioData, 0, size);

        AudioClip clip = AudioClip.Create(basename, size /  reader.WaveFormat.Channels, reader.WaveFormat.Channels, reader.WaveFormat.SampleRate, false);
        clip.SetData(audioData, 0);
        player.clip = clip;
        player.Play();
    }

    private void RenderMusic(string path)
    {
        WaveFormRenderer renderer = new WaveFormRenderer();
        WaveFormRendererSettings settings = new StandardWaveFormRendererSettings
        {
            Width = 640,
            TopHeight = 32,
            BottomHeight = 32   
        };
        System.Drawing.Image img = renderer.Render(path, new MaxPeakProvider(), settings);

        string imgpath = Application.temporaryCachePath + Path.GetFileNameWithoutExtension(path) + ".png";
        img.Save(imgpath, System.Drawing.Imaging.ImageFormat.Png);
        Debug.Log(imgpath);

        byte[] byteArray = File.ReadAllBytes(imgpath);
        Texture2D texture = new Texture2D(0,0);
        bool isLoaded = texture.LoadImage(byteArray);
        if (isLoaded)
        {
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
        }
    }
}