using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static bool isPaused = true;

    public static void GamePause(bool state)
    {
        isPaused = state;
        Time.timeScale = state ? 0 : 1;
    }

    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
