using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneHistory
{
    private static Stack<string> history = new Stack<string>();

    #region LoadScene

    public static void LoadScene(int sceneBuildIndex)
    {
        history.Push(SceneManager.GetActiveScene().path);
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public static void LoadScene(string sceneName)
    {
        history.Push(SceneManager.GetActiveScene().path);
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadScene(int sceneBuildIndex, LoadSceneMode mode)
    {
        history.Push(SceneManager.GetActiveScene().path);
        SceneManager.LoadScene(sceneBuildIndex, mode);
    }

    public static void LoadScene(string sceneName, LoadSceneMode mode)
    {
        history.Push(SceneManager.GetActiveScene().path);
        SceneManager.LoadScene(sceneName, mode);
    }

    public static void LoadScene(int sceneBuildIndex, LoadSceneParameters parameters)
    {
        history.Push(SceneManager.GetActiveScene().path);
        SceneManager.LoadScene(sceneBuildIndex, parameters);
    }

    public static void LoadScene(string sceneName, LoadSceneParameters parameters)
    {
        history.Push(SceneManager.GetActiveScene().path);
        SceneManager.LoadScene(sceneName, parameters);
    }

    #endregion

    #region LoadPreviousScene

    public static void LoadPreviousScene()
    {
        if (history.Count > 0)
            SceneManager.LoadScene(history.Pop());
    }

    public static void LoadPreviousScene(LoadSceneMode mode)
    {
        if (history.Count > 0)
            SceneManager.LoadScene(history.Pop(), mode);
    }
    
    public static void LoadPreviousScene(LoadSceneParameters parameters)
    {
        if (history.Count > 0)
            SceneManager.LoadScene(history.Pop(), parameters);
    }

    #endregion
}
