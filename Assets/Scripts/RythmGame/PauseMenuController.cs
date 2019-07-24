using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu = null;

    [SerializeField]
    private AudioSource audioPlayer = null;

    bool paused = false;
    float previousTimeScale = 1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (!paused)
        {
            paused = true;
            previousTimeScale = Time.timeScale;
            audioPlayer.Pause();
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        if (paused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = previousTimeScale;
            audioPlayer.UnPause();
            paused = false;
        }
    }

    public void Retry()
    {
        if (paused)
        {
            Time.timeScale = previousTimeScale;
        }
        SceneHistory.ReloadActiveScene();
    }

    public void Quit()
    {
        if (paused)
        {
            Time.timeScale = previousTimeScale;
        }
        SceneHistory.LoadPreviousScene();
    }
}
