using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject PanelPause;


    void Start()
    {


        if (PanelPause == null)
        {
            return;
        }
        PanelPause.SetActive(false);
        GameManager.GamePause(false);

    }

    public void PauseGame()
    {
        PanelPause.SetActive(true);
        GameManager.GamePause(true);
    }
    public void ResumeGame()
    {
        PanelPause.SetActive(false);
        GameManager.GamePause(false);
    }

    public void ChangeScene(string sceneName)
    {
        GameManager.ChangeScene(sceneName);
    }

    public void QuitGame()
    {
        GameManager.QuitGame();
    }

}
