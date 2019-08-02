using UnityEngine.UI;
using DancingICE.RythmGame;

public class PlayButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => SceneHistory.LoadScene(RythmGameSettings.GameMode.gameScene));
    }

    private void Update()
    {
        interactable = (RythmGameSettings.BeatmapToLoad != null);
    }
}
