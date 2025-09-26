# 📘 Documentación Trivia  

## 🎯 Uso del patrón Strategy en los modos de juego  

En este proyecto implementé el **patrón de diseño Strategy** para manejar los diferentes **modos de juego** de la trivia.  

La razón principal fue la **escalabilidad**:  
- Evité que toda la lógica de los modos de juego estuviera mezclada dentro del `TriviaController`.  
- Con `Strategy`, cada modo de juego implementa la misma interfaz (`IGameModeStrategy`), lo que permite **cambiar de modo fácilmente** sin alterar el núcleo del controlador.  
- A futuro, puedo tener una **variable estática** (por ejemplo, definida en el `GameManager` o en la pantalla de selección de modo) que decida cuál `Strategy` cargar.  
- Agregar un nuevo modo será tan simple como **crear otra clase** que implemente `IGameModeStrategy`.  

Esto hace que el código sea:  
✔️ Más ordenado  
✔️ Fácil de mantener  
✔️ Abierto a la extensión (añadir más modos) sin romper lo ya hecho  

---

## 🧩 Estructura básica  

La interfaz `IGameModeStrategy` define los métodos base que cada modo debe implementar.  
Más abajo encontrarás el código completo.  

---

# 🏆 Sistema de Puntajes y Game Over  

Además del sistema de modos, también desarrollé un **sistema de puntuaciones con tablas de high scores** y una pantalla de **Game Over**.  
La idea es dar **motivación al jugador** mostrando sus logros y guardando los mejores resultados en cada modo de juego.  

---

## 🎯 Objetivo del sistema  
- Guardar el puntaje actual y las mejores puntuaciones.  
- Diferenciar entre puntajes de **modo por ronda** y **modo por pregunta**.  
- Mostrar las listas de los **top 5 jugadores** (por cada modo).  
- Dar un **mensaje motivacional** al terminar la partida.  

---

## ⚙️ Scripts principales  

### 1. `ScoreManager` (Gestión de puntajes)  

Este script es un **singleton estático** que controla toda la lógica de puntajes:  

- **Variables clave**  
  - `CurrentScore`: almacena el puntaje actual.  
  - `RoundScoresKey` y `QuestionScoresKey`: claves para guardar en `PlayerPrefs`.  
  - `MaxScores`: máximo de posiciones en el ranking (5).  

- **Funciones principales**  
  - `ResetScore()`: reinicia el puntaje al empezar una partida.  
  - `AddScore(int points)`: suma puntos al marcador.  
  - `SaveScore(string playerName, ScoreType type)`: guarda el puntaje en la tabla correspondiente (ronda/pregunta).  
  - `LoadScores(ScoreType type)`: carga las puntuaciones guardadas desde `PlayerPrefs`.  

Los puntajes se guardan en JSON con la ayuda de las clases `ScoreEntry` y `ScoreList`.  

---

### 2. `ScoreUi` (Mostrar las tablas de puntajes)  

- **Objetivo**: Renderizar en pantalla los puntajes guardados.  
- **Funcionamiento**:  
  - Se instancian prefabs de filas de texto (`scoreEntryPrefab`).  
  - Cada fila muestra el puesto, nombre del jugador y puntaje.  
  - Se inicializan 6 filas (para asegurar siempre espacio en la UI).  
  - Usa `ShowHighScores(ScoreType type)` para actualizar la lista según el modo.  


### 1. `GameModeManager`
Clase estática que funciona como **contenedor global** para el modo de juego seleccionado.

- Guarda en `selectedMode` el número del modo elegido.  
- Expone el método `SetMode(int mode)` para actualizar ese valor.  
- Permite que la escena de trivia sepa en qué modo debe iniciar.  

👉 Sirve como "puente" entre el menú principal y la escena de trivia.

---

### 2. `TriviaController`
Es el **cerebro de la trivia**: maneja preguntas, UI, puntaje, feedback y modos de juego.

#### 🔹 Funciones principales
- **UI**: controla textos de pregunta, opciones, puntaje, temporizador, mensajes y paneles (`GameOverPanel`, `PauseButton`).  
- **Preguntas y categorías**: carga las preguntas desde `TriviaLoader` y las organiza por categorías.  
- **Estrategias de juego (Strategy)**:  
  - `LearningMode`  
  - `PerQuestionTimerMode`  
  - `PerRoundTimerMode`  
- **Respuestas**: asigna las opciones a los botones y al seleccionar una:  
  - Desactiva los botones.  
  - Muestra feedback visual y auditivo.  
  - Pasa a la siguiente pregunta con un pequeño delay.  
- **Feedback**:  
  - Correcto → cambia a verde, muestra un mensaje motivacional aleatorio, reproduce sonido de acierto.  
  - Incorrecto → muestra "Inténtalo de nuevo" en rojo, reproduce sonido de error.  
- **Puntaje**: se suma usando `ScoreManager` y se refleja en la UI.  
- **Temporizador**: actualizado por el modo de juego activo.  
- **Final de partida**: muestra "Juego Terminado", detiene el timer y enseña el panel de Game Over con el puntaje.  

👉 En resumen: orquesta toda la lógica de la trivia y se adapta al modo seleccionado sin mezclar responsabilidades.

---

### 3. `MenuUIController`
Controlador del **menú principal**.

- Cuando el jugador elige un modo, lo guarda en `GameModeManager.SetMode`.  
- Luego carga la escena `"Trivia"`.  

👉 Es el **puente entre el menú y el inicio del juego**.

---

👉 Ejemplo de visualización:  


---

# 💻 Código de referencia  

Aquí se incluyen los scripts clave usados en el proyecto:  

### `IGameModeStrategy.cs`  
```csharp
public interface IGameModeStrategy
{
    void StartGame(TriviaController controller);
    void OnAnswerSelected(bool isCorrect);
    void Update();
    void EndGame();
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int CurrentScore { get; private set; }
    private const string RoundScoresKey = "RoundScores";
    private const string QuestionScoresKey = "QuestionScores";
    private const int MaxScores = 5;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }

    public void AddScore(int points)
    {
        CurrentScore += points;
    }

    public void SaveScore(string playerName, ScoreType type)
    {
        // Lógica de guardado en JSON con PlayerPrefs
    }

    public List<ScoreEntry> LoadScores(ScoreType type)
    {
        // Lógica de carga de datos
        return new List<ScoreEntry>();
    }
}

public class ScoreUi : MonoBehaviour
{
    [SerializeField] private GameObject scoreEntryPrefab;
    [SerializeField] private Transform scoreContainer;

    public void ShowHighScores(ScoreType type)
    {
        var scores = ScoreManager.Instance.LoadScores(type);

        foreach (Transform child in scoreContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < scores.Count; i++)
        {
            var entry = Instantiate(scoreEntryPrefab, scoreContainer);
            entry.GetComponent<Text>().text = $"{i + 1}. {scores[i].playerName} - {scores[i].score}";
        }
    }
}

// GameModeManager.cs
using UnityEngine;

public static class GameModeManager
{
    public static int selectedMode = 0;

    public static void SetMode(int mode)
    {
        selectedMode = mode;
    }
}

// TriviaController.cs
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

            // Sonido de respuesta correcta
            SoundManager.Instance?.PlayCorrect();
        }
        else
        {
            // Restaurar color normal
            questionText.color = normalColor;
            messageText.text = "Inténtalo de nuevo";
            messageText.color = Color.red;

            // Sonido de respuesta incorrecta
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

// MenuUIController.cs
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
