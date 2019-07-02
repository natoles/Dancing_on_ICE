using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneButton : Button, IPointerClickHandler
{
    [SerializeField]
    private string ScenePath = null;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        SceneManager.LoadScene(ScenePath);
    }
}
