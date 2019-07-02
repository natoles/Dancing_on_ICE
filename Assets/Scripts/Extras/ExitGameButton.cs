using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitGameButton : Button, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        Application.Quit();
    }
}
