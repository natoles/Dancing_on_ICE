using UnityEngine;
using DancingICE.RythmGame;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [System.NonSerialized] 
    public RythmGameController rythmGameController = null;

    [SerializeField]
    private Button ResumeButton = null;

    [SerializeField]    
    private Button RetryButton = null;

    [SerializeField]    
    private Button QuitButton = null;

    protected void Start()
    {
        if (ResumeButton != null)
            ResumeButton.onClick.AddListener(rythmGameController.Resume);
        if (RetryButton != null)
            RetryButton.onClick.AddListener(rythmGameController.Retry);
        if (QuitButton != null)
            QuitButton.onClick.AddListener(rythmGameController.Quit);
    }
}
