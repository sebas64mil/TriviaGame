using UnityEngine;

public class LearningMode : IGameModeStrategy
{
    private TriviaController controller;
    private LearningModeUI learningUI;

    private int currentStep = 0;
    private bool tutorialActive = false;

    private PerQuestionTimerMode perQuestionTimerMode;

    public void StartGame(TriviaController controller)
    {
        this.controller = controller;
        learningUI = controller.learningUI;

        if (learningUI == null)
        {
            Debug.LogError("No se asignó LearningModeUI en el TriviaController.");
            return;
        }

        perQuestionTimerMode = new PerQuestionTimerMode();

        // ----------------- DESACTIVAR BOTONES DURANTE TUTORIAL -----------------
        controller.SetButtonsInteractable(false);

        // Activar tutorial
        learningUI.tutorialPanel.SetActive(true);
        learningUI.nextButton.gameObject.SetActive(true);

        currentStep = 0;
        ShowCurrentStep();

        tutorialActive = true;

        // Pausar con GameManager, no directo
        GameManager.GamePause(true);

        learningUI.nextButton.onClick.RemoveAllListeners();
        learningUI.nextButton.onClick.AddListener(NextStep);
    }

    private void ShowCurrentStep()
    {
        if (currentStep >= learningUI.steps.Length)
        {
            EndTutorial();
            return;
        }

        TutorialStep step = learningUI.steps[currentStep];
        learningUI.tutorialPanel.SetActive(true);
        learningUI.tutorialText.text = step.text;
        learningUI.tutorialPanel.GetComponent<RectTransform>().anchoredPosition = step.position;
    }

    private void NextStep()
    {
        currentStep++;
        ShowCurrentStep();
    }

    private void EndTutorial()
    {
        learningUI.tutorialPanel.SetActive(false);
        learningUI.nextButton.gameObject.SetActive(false);

        tutorialActive = false;

        // ----------------- VOLVER A ACTIVAR BOTONES AL TERMINAR TUTORIAL -----------------
        controller.SetButtonsInteractable(true);

        // Reanudar juego desde GameManager
        GameManager.GamePause(false);

        perQuestionTimerMode.StartGame(controller);
        controller.LoadQuestion();
    }

    public bool IsTutorialActive()
    {
        return tutorialActive;
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (tutorialActive) return;
        perQuestionTimerMode.OnAnswerSelected(isCorrect);
    }

    public void Update()
    {
        if (tutorialActive)
        {
            if (GameManager.isPaused)
            {
                learningUI.tutorialPanel.SetActive(false);
                learningUI.nextButton.gameObject.SetActive(false);
            }
            else
            {
                learningUI.tutorialPanel.SetActive(true);
                learningUI.nextButton.gameObject.SetActive(true);
            }
            return;
        }

        if (!GameManager.isPaused)
        {
            perQuestionTimerMode.Update();
        }
    }

    public void EndGame()
    {
        if (tutorialActive)
        {
            if (learningUI != null)
            {
                learningUI.tutorialPanel.SetActive(false);
                learningUI.nextButton.gameObject.SetActive(false);
            }
            GameManager.GamePause(false);
            tutorialActive = false;
        }

        perQuestionTimerMode.EndGame();
    }
}
