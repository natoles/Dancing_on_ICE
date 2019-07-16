using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RefreshSongsButton : Button
{
    [SerializeField]
    private SongScrollView Scroll = null;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => { Scroll.UpdateSongsList(); Scroll.SelectRandomSong(); });
    }
}
