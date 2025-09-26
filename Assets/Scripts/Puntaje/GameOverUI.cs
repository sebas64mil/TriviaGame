using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text gameOverText;

    // Se agrega el ScoreType para guardar en la lista correcta
    public void ShowGameOver(string playerName, ScoreType type)
    {
        int finalScore = ScoreManager.CurrentScore;

        // Guardar score en la lista correspondiente
        ScoreManager.SaveScore(playerName, type);

        // Mensajes según el puntaje
        if (finalScore >= 100)
        {
            gameOverText.text = "¡Increíble! ";
        }
        else if (finalScore >= 50)
        {
            gameOverText.text = "¡Muy bien hecho! ";
        }
        else
        {
            gameOverText.text = "¡Sigue practicando! ";
        }

        // Mostrar puntaje final
        gameOverText.text += $"{playerName}: {finalScore} puntos";
    }
}
