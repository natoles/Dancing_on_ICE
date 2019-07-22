using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomSongButton : Button
{
    [SerializeField]
    private SongScrollView Scroll = null;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(Scroll.SelectRandomSong);
    }
}
