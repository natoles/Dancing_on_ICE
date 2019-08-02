using DancingICE.RythmGame;

public class PlayButton : LoadSceneButton
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => transform.parent.gameObject.SetActive(false));
    }

    private void Update()
    {
        interactable = (RythmGameSettings.BeatmapToLoad != null);
    }
}
