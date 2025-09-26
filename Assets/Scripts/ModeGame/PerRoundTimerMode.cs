using UnityEngine;

public class PerRoundTimerMode : IGameModeStrategy
{
    private TriviaController controller;
    private float roundTime = 60f; // segundos por ronda
    private bool gameActive = false;

    public void StartGame(TriviaController controller)
    {
        this.controller = controller;
        ResetRoundTimer();
        gameActive = true;
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (!gameActive) return;

        if (isCorrect)
            controller.AddScore(10);
        else
            controller.AddScore(0);

    }

    public void Update()
    {
        if (!gameActive) return;

        roundTime -= Time.deltaTime;
        controller.UpdateTimer(roundTime);

        if (roundTime <= 0f)
        {
            controller.ShowMessage("¡Fin de la ronda!", Color.yellow);
            controller.EndTrivia();
        }
    }

    public void EndGame()
    {
        gameActive = false;
        controller.GameOverPanel.SetActive(true);
        controller.PauseButton.gameObject.SetActive(false);
    }

    public void ResetRoundTimer()
    {
        roundTime = 60f;
        controller.UpdateTimer(roundTime);
    }
}
