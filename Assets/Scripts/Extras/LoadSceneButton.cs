using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneButton : Button
{
    [SerializeField]
    private string ScenePath = null;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => SceneManager.LoadScene(ScenePath));
    }
}
