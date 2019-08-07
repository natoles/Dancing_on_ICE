using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeactivateParentButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => transform.parent.gameObject.SetActive(false));
    }
}