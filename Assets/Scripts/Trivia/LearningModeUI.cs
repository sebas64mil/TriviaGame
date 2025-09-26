using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct TutorialStep
{
    public string text;          // Texto explicativo
    public Vector2 position;     // Posici�n del panel en la pantalla
}

public class LearningModeUI : MonoBehaviour
{
    [Header("Panel Tutorial")]
    public GameObject tutorialPanel;   // Panel con fondo tipo globo
    public TMP_Text tutorialText;      // Texto dentro del panel

    [Header("Bot�n Avanzar")]
    public Button nextButton;          // Bot�n que avanza al siguiente paso

    [Header("Pasos de Tutorial")]
    public TutorialStep[] steps;       // Arreglo de pasos
}
