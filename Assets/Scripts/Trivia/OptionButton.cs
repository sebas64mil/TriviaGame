using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionButton : MonoBehaviour
{
    private TMP_Text optionText;
    private int optionIndex;
    private int correctIndex;

    public void Start()
    {
        optionText = GetComponentInChildren<TMP_Text>();
    }
    public void Setup(string text, int optionIndex, int correctIndex)
    {
        this.optionIndex = optionIndex;
        this.correctIndex = correctIndex;
        optionText.text = text;
    }

    public void OnClick()
    {
        bool isCorrect = (optionIndex == correctIndex);
        TriviaEvents.OnAnswerSelected?.Invoke(isCorrect);
    }
}
