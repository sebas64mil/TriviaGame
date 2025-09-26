public interface IGameModeStrategy
{
    void StartGame(TriviaController controller);
    void OnAnswerSelected(bool isCorrect);
    void Update();
    void EndGame();
}
