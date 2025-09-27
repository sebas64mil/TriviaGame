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

# 📘 RegisterScreen en Unity

Este script controla la **pantalla de registro de usuario** en Unity.  
Incluye validación de **nombre**, **correo**, manejo de **fotos (galería/cámara)** y transición entre **paneles de UI**.

---

## 🔹 Variables expuestas al Inspector

### 📥 Entradas de texto
- Nombre del jugador.  
- Correo electrónico.

### 🖼️ Foto
- Vista previa de la foto seleccionada/tomada.

### 📢 Mensajes
- Texto donde se muestran mensajes.  
- Colores para mensajes de éxito, error y advertencia.

### 📂 Paneles de UI
- Panel de registro.  
- Panel posterior al registro.

### 📝 Mostrar nombre
- Texto para mostrar el nombre del jugador guardado.

---

## ⚙️ Variables internas

- Foto seleccionada.  
- Cámara en PC.  
- Bandera para indicar si el jugador ya se registró.

---

## 🚀 Registro (OnRegister)

1. Lee el nombre y correo del jugador.  
2. Valida:  
   - Nombre no vacío.  
   - Al menos 3 caracteres.  
   - Solo letras y números.  
   - Que no esté repetido.  
   - Correo válido.  
   - Foto seleccionada.  
3. Si todo está bien:  
   - Muestra mensaje de éxito.  
   - Guarda el nombre en PlayerPrefs.  
   - Actualiza el texto con el nombre del jugador.

---

## ⏭️ Continuar (OnContinue)

- Si no está registrado → muestra advertencia.  
- Si está registrado:  
  - Oculta el panel de registro.  
  - Activa el panel de continuación.  
  - Recupera y muestra el nombre desde PlayerPrefs.

---

## 🔍 Funciones auxiliares

- Validar que el nombre solo tenga letras y números.  
- Verificar si el nombre ya está guardado en PlayerPrefs.  
- Validar el formato del correo electrónico.  
- Mostrar mensajes con texto y color en la UI.

---

## 📷 Fotos

### Desde galería
- Disponible en Android/iOS con NativeGallery.  
- Permite seleccionar una foto de la galería y mostrarla en la vista previa.

### Desde cámara
- En Android/iOS usa NativeCamera.  
- En PC:  
  - Activa la cámara la primera vez.  
  - Captura foto la segunda vez.  



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

using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro
using System.Text.RegularExpressions;

public class RegisterScreen : MonoBehaviour
{
    [Header("UI Inputs")]
    public TMP_InputField inputName;
    public TMP_InputField inputEmail;

    [Header("UI Photo")]
    public RawImage photoPreview;

    [Header("UI Messages")]
    public TMP_Text messageText;

    [Header("Message Colors")]
    public Color successColor = Color.green;
    public Color errorColor = Color.red;
    public Color warningColor = Color.yellow;

    [Header("Object Actives")]
    public GameObject registerPanel;
    public GameObject continuePanel;

    [Header("Player Name Display")]
    public TMP_Text playerNameText; // <- Texto donde mostraremos el nombre guardado

    private Texture2D selectedPhoto;
    private WebCamTexture webCam;

    // Flag para saber si se registró
    private bool isRegistered = false;

    // -------------------
    // VALIDAR Y REGISTRAR
    // -------------------

    public void OnRegister()
    {
        string name = inputName.text.Trim();
        string email = inputEmail.text.Trim();

        // Validaciones del nombre
        if (string.IsNullOrEmpty(name))
        {
            ShowMessage("El nombre es obligatorio", errorColor);
            return;
        }

        if (name.Length < 3)
        {
            ShowMessage("El nombre debe tener al menos 3 caracteres", errorColor);
            return;
        }

        if (!IsValidName(name))
        {
            ShowMessage("El nombre solo puede contener letras y números", errorColor);
            return;
        }

        if (IsNameTaken(name))
        {
            ShowMessage("El nombre ya está en uso", errorColor);
            return;
        }

        // Validaciones del email
        if (string.IsNullOrEmpty(email))
        {
            ShowMessage("El correo es obligatorio", errorColor);
            return;
        }

        if (!IsValidEmail(email))
        {
            ShowMessage("Formato de correo inválido", errorColor);
            return;
        }

        if (selectedPhoto == null)
        {
            ShowMessage("Debes seleccionar o tomar una foto", errorColor);
            return;
        }

        // Si pasa todas las validaciones
        ShowMessage("Registro exitoso", successColor);
        isRegistered = true;

        // Guardamos el nombre en PlayerPrefs
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();

        // Mostrar el nombre de inmediato
        if (playerNameText != null)
            playerNameText.text = name;
    }

    // -------------------
    // FUNCIONES AUXILIARES
    // -------------------
    private bool IsValidName(string name)
    {
        // Solo letras y números
        string pattern = @"^[a-zA-Z0-9]+$";
        return Regex.IsMatch(name, pattern);
    }

    private bool IsNameTaken(string name)
    {
        // Aquí solo comprobamos el nombre guardado en PlayerPrefs
        string savedName = PlayerPrefs.GetString("PlayerName", "");
        return savedName.Equals(name, System.StringComparison.OrdinalIgnoreCase);
    }

    public void OnContinue()
    {
        if (!isRegistered)
        {
            ShowMessage("Primero debes registrarte antes de continuar", warningColor);
            return;
        }

        // Cambiar de panel
        registerPanel.SetActive(false);
        continuePanel.SetActive(true);

        // Recuperamos el nombre y lo mostramos
        if (playerNameText != null)
        {
            string savedName = PlayerPrefs.GetString("PlayerName", "Jugador");
            playerNameText.text = savedName;
        }
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    private void ShowMessage(string text, Color color)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.color = color;
        }
    }

    // -------------------
    // FOTO: DESDE GALERÍA
    // -------------------
    public void PickFromGallery()
    {
#if UNITY_ANDROID || UNITY_IOS
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512, false);
                if (texture != null)
                {
                    selectedPhoto = texture;
                    photoPreview.texture = texture;
                }
                else
                {
                    ShowMessage("No se pudo cargar la imagen", errorColor);
                }
            }
        }, "Selecciona una imagen", "image/*");
#else
        ShowMessage("La galería solo está disponible en Android/iOS", warningColor);
#endif
    }

    // -------------------
    // FOTO: DESDE CÁMARA
    // -------------------
    public void TakePhoto()
    {
#if UNITY_ANDROID || UNITY_IOS
        NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 512, false);
                if (texture != null)
                {
                    selectedPhoto = texture;
                    photoPreview.texture = texture;
                }
                else
                {
                    ShowMessage("No se pudo tomar la foto", errorColor);
                }
            }
        }, 512);
#else
        if (webCam == null)
        {
            webCam = new WebCamTexture();
            photoPreview.texture = webCam;
            webCam.Play();
            ShowMessage("Cámara activada. Pulsa de nuevo para capturar.", warningColor);
        }
        else if (webCam.isPlaying)
        {
            Texture2D photo = new Texture2D(webCam.width, webCam.height);
            photo.SetPixels(webCam.GetPixels());
            photo.Apply();

            selectedPhoto = photo;
            photoPreview.texture = photo;

            webCam.Stop();
            webCam = null;

            ShowMessage("Foto tomada con la webcam", successColor);
        }
#endif
    }
}

