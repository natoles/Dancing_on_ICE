using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : Button
{
    [SerializeField]
    private string ScenePath = null;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => SceneHistory.LoadScene(ScenePath));
    }
}
