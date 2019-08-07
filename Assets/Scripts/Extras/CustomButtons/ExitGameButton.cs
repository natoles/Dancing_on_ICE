using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitGameButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(Application.Quit);
    }
}
