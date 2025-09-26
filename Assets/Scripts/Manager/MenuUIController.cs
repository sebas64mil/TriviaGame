using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    public void OnSelectMode(int mode)
    {
        GameModeManager.SetMode(mode);
        SceneManager.LoadScene("Trivia"); // carga la escena del juego
    }
}
