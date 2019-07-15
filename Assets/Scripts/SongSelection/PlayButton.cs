using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButton : LoadSceneButton
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => transform.parent.gameObject.SetActive(false));
    }

    private void Update()
    {
        interactable = (TwitchRythmController.BeatmapToLoad != null);
    }
}
