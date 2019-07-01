using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomSongButton : Button, IPointerClickHandler
{
    [SerializeField]
    private SongScrollView Scroll = null;

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        base.OnPointerClick(pointerEventData);
        Scroll.SelectRandomSong();
    }
}
