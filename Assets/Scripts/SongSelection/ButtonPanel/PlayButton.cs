using UnityEngine;
using UnityEngine.UI;
using DancingICE.RythmGame;

public class PlayButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying)
            onClick.AddListener(() => SceneHistory.LoadScene(RythmGameSettings.GameMode.gameScene));
    }

    private void Update()
    {
        if (Application.isPlaying)
            interactable = (RythmGameSettings.BeatmapToLoad != null) && (!RythmGameSettings.GameMode.useTwitchIntegration || TwitchClient.Instance.IsConnected);
    }
}
