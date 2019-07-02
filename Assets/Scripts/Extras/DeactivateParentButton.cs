using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeactivateParentButton : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        transform.parent.gameObject.SetActive(false);
    }
}