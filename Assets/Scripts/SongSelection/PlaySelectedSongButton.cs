using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaySelectedSongButton : Button, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        base.OnPointerClick(pointerEventData);
        if (TwitchRythmController.beatmapToLoad != null)
        {
            SceneManager.LoadScene("TwitchMode");
        }
    }
}
