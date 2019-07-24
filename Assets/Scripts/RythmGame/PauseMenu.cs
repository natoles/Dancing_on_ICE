using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    bool paused;
    float previousTimeScale;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        if (!paused)
        {
            paused = true;
            previousTimeScale = Time.timeScale;
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
