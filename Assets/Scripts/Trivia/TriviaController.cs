using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TriviaController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text questionText;
    public List<Button> optionButtons; // botones normales
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text messageText;

    public GameObject GameOverPanel;
    public Button PauseButton;

    [Header("Learning Mode UI")]
    public LearningModeUI learningUI; // referencia al UI del modo aprendizaje

    [Header("Game Over UI")]
    public GameOverUI gameOverUI;

    [Header("Feedback Settings")]
    public Color correctColor = Color.green;
    public Color normalColor = Color.white;

    // Mensajes motivacionales
    public string[] motivationalMessages = {
        "¡Excelente!",
        "¡Sigue así!",
        "¡Muy bien!",
        "¡Impresionante!",
        "¡Genial!"
    };

    private IGameModeStrategy gameMode;
    private TriviaData triviaData;
    private List<QuestionData> currentQuestions;
    private int currentIndex = 0;
    private int currentCategoryIndex = 0; // índice de la categoría actual

    private bool gameEnded = false;

    private void Start()
    {
        // Inicializar UI
        scoreText.text = "Puntaje: 0";
        timerText.text = "Tiempo: 0";
        messageText.text = "";

        GameOverPanel.SetActive(false);
        PauseButton.gameObject.SetActive(true);

        // Cargar todas las preguntas por categoría
        triviaData = TriviaLoader.LoadQuestions();
        LoadCategory(currentCategoryIndex);

        // Asignar estrategia (ejemplo: Timer por ronda)
        switch (GameModeManager.selectedMode)
        {
            case 0:
                gameMode = new LearningMode();
                break;
            case 1:
                gameMode = new PerQuestionTimerMode();
                break;
            case 2:
                gameMode = new PerRoundTimerMode();
                break;
            default:
                gameMode = new PerQuestionTimerMode(); // fallback
                break;
        }

        gameMode.StartGame(this);

        LoadQuestion();
    }

    private void Update()
    {
        if (!gameEnded)
            gameMode.Update();
    }

    private void LoadCategory(int categoryIndex)
    {
        if (categoryIndex >= triviaData.categories.Length)
        {
            EndTrivia();
            return;
        }

        currentQuestions = new List<QuestionData>(triviaData.categories[categoryIndex].questions);
        currentIndex = 0;

        // Aquí la estrategia puede reiniciar el timer si quiere
        if (gameMode is PerRoundTimerMode roundMode)
        {
            roundMode.ResetRoundTimer();
        }
    }

    public void LoadQuestion()
    {
        if (currentIndex >= currentQuestions.Count)
        {
            // Fin de la categoría, pasar a la siguiente
            currentCategoryIndex++;
            LoadCategory(currentCategoryIndex);
            if (gameEnded) return;
        }

        QuestionData q = currentQuestions[currentIndex];
        questionText.text = q.question;
        questionText.color = normalColor; // reset color al cargar pregunta
        messageText.text = ""; // limpiar mensaje al cargar nueva pregunta

        for (int i = 0; i < optionButtons.Count; i++)
        {
            TMP_Text btnText = optionButtons[i].GetComponentInChildren<TMP_Text>();
            btnText.text = q.options[i];

            // ----------------- ACTIVAR BOTONES SOLO SI NO HAY TUTORIAL -----------------
            optionButtons[i].interactable = !IsTutorialActive();

            int optionIndex = i; // closure
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() =>
            {
                bool isCorrect = optionIndex == q.correctIndex;

                // DESACTIVAR BOTONES AL RESPONDER
                SetButtonsInteractable(false);

                // Mostrar feedback visual
                ProvideFeedback(isCorrect);

                StartCoroutine(WaitAndNext(isCorrect));
            });
        }
    }

    // ----------------- MÉTODO AUXILIAR PARA SABER SI ESTAMOS EN TUTORIAL -----------------
    private bool IsTutorialActive()
    {
        return gameMode is LearningMode learning && learning.IsTutorialActive();
    }

    // Corutina para esperar antes de la siguiente pregunta
    private IEnumerator WaitAndNext(bool isCorrect)
    {
        // Actualizar puntaje
        gameMode.OnAnswerSelected(isCorrect);

        // Esperar un momento para que el jugador vea el mensaje
        yield return new WaitForSeconds(0.8f);

        // Limpiar mensaje y color de la pregunta
        messageText.text = "";
        questionText.color = normalColor;

        // ----------------- VOLVER A ACTIVAR BOTONES -----------------
        LoadNextQuestion();
    }

    // Método auxiliar para activar/desactivar botones
    public void SetButtonsInteractable(bool state)
    {
        foreach (var btn in optionButtons)
        {
            btn.interactable = state;
        }
    }

    // ------------------ NUEVO MÉTODO DE FEEDBACK ------------------
    private void ProvideFeedback(bool isCorrect)
    {
        if (isCorrect)
        {
            // Cambiar color de la pregunta
            questionText.color = correctColor;

            // Mensaje motivacional aleatorio
            int index = Random.Range(0, motivationalMessages.Length);
            messageText.text = motivationalMessages[index];
            messageText.color = correctColor;

            // ?? Sonido de respuesta correcta
            SoundManager.Instance?.PlayCorrect();
        }
        else
        {
            // Restaurar color normal
            questionText.color = normalColor;
            messageText.text = "Inténtalo de nuevo";
            messageText.color = Color.red;

            // ?? Sonido de respuesta incorrecta
            SoundManager.Instance?.PlayWrong();
        }
    }


    public void AddScore(int points)
    {
        ScoreManager.AddScore(points);
        scoreText.text = "Puntaje: " + ScoreManager.CurrentScore;
    }

    public void LoadNextQuestion()
    {
        currentIndex++;
        LoadQuestion();
    }

    public void UpdateTimer(float timeLeft)
    {
        timerText.text = "" + Mathf.CeilToInt(timeLeft);
    }

    public void ShowMessage(string msg, Color color)
    {
        messageText.text = msg;
        messageText.color = color;
    }

    public void EndTrivia()
    {
        gameEnded = true; // detiene el Update del timer
        gameMode.EndGame();
        ShowMessage("Juego Terminado", Color.yellow);

        // Mostrar UI de Game Over
        string playerName = PlayerPrefs.GetString("PlayerName", "Jugador");
        ScoreType typeToSave = (gameMode is PerRoundTimerMode) ? ScoreType.Round : ScoreType.Question;
        gameOverUI.ShowGameOver(playerName, typeToSave);

        timerText.text = "Tiempo: 0";
    }
}
