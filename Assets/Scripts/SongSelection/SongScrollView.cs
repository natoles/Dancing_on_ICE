using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SongScrollView : ScrollRect
{
    [SerializeField]
    protected SongEntry m_SongEntry = null;

    public int SongCount = 0;

    protected override void Start()
    {
        if (Application.isPlaying)
        {
            string[] songFolders = Directory.GetDirectories(Application.streamingAssetsPath + "/Songs");
            foreach (string songFolder in songFolders)
            {
                string[] bmFiles = Directory.GetFiles(songFolder, "*.icebm");
                foreach (string bmFile in bmFiles)
                {
                    SongEntry entry = Instantiate(m_SongEntry, content);
                    entry.gameObject.name = SongCount + " - " + Path.GetFileNameWithoutExtension(bmFile);
                    entry.SetSong(SongCount, bmFile);
                    SongCount++;
                }
            }
        }
    }
}