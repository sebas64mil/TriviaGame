using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreUi : MonoBehaviour
{
    [Header("UI por Ronda")]
    public Transform roundScoreContainer;
    public GameObject scoreEntryPrefab;

    [Header("UI por Pregunta")]
    public Transform questionScoreContainer;
    public GameObject questionScoreEntryPrefab;

    private List<TMP_Text> roundEntries = new List<TMP_Text>();
    private List<TMP_Text> questionEntries = new List<TMP_Text>();

    private void Start()
    {
        InitializeEntries(roundScoreContainer, scoreEntryPrefab, roundEntries);
        InitializeEntries(questionScoreContainer, questionScoreEntryPrefab, questionEntries);

        // Mostrar puntajes al inicio para test
        ShowHighScores(ScoreType.Round);
        ShowHighScores(ScoreType.Question);
    }

    private void InitializeEntries(Transform container, GameObject prefab, List<TMP_Text> list)
    {
        int max = 6;
        for (int i = 0; i < max; i++)
        {
            GameObject row = Instantiate(prefab, container);
            TMP_Text text = row.GetComponentInChildren<TMP_Text>();

            if (text == null)
                Debug.LogError($"[ScoreUi] TMP_Text no encontrado en prefab {prefab.name}");

            text.text = $"{i + 1}. -";
            list.Add(text);
        }
    }

    // Nuevo método usando ScoreType
    public void ShowHighScores(ScoreType type)
    {
        var scores = ScoreManager.LoadScores(type);

        if (type == ScoreType.Round)
            UpdateEntries(scores, roundEntries);
        else
            UpdateEntries(scores, questionEntries);
    }

    private void UpdateEntries(List<ScoreEntry> scores, List<TMP_Text> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (i < scores.Count)
            {
                entries[i].text = $"{i + 1}. {scores[i].playerName} — {scores[i].score}";
            }
            else
            {
                entries[i].text = $"{i + 1}. -";
            }
        }
    }
}
