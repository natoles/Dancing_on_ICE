using UnityEngine.UI;

public class LoadPreviousSceneButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => SceneHistory.LoadPreviousScene());
    }
}
