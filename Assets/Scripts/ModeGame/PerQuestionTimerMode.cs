using UnityEngine;

public class PerQuestionTimerMode : IGameModeStrategy
{
    private TriviaController controller;
    private float timeLeft = 12f; // segundos por pregunta
    private bool gameActive = false;

    public void StartGame(TriviaController controller)
    {
        this.controller = controller;
        ResetTimer();
        gameActive = true;
        Debug.Log("Modo Timer por Pregunta iniciado");
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (!gameActive) return;

        if (isCorrect)
            controller.AddScore(10);
        else
            controller.AddScore(0);

        ResetTimer();
    }

    public void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        controller.UpdateTimer(timeLeft);

        if (timeLeft <= 0f)
        {
            controller.ShowMessage("¡Tiempo agotado!", Color.red);
            controller.LoadNextQuestion();
            ResetTimer();
        }
    }

    public void EndGame()
    {
        gameActive = false;
        controller.GameOverPanel.SetActive(true);
        controller.PauseButton.gameObject.SetActive(false);
        Debug.Log("Fin del modo Timer por Pregunta");
    }

    private void ResetTimer()
    {
        timeLeft = 10f;
        controller.UpdateTimer(timeLeft);
    }
}
