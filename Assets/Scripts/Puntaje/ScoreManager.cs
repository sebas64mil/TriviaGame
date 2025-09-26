using UnityEngine;
using System.Collections.Generic;

// Aquí defines el enum
public enum ScoreType
{
    Round,
    Question
}

public static class ScoreManager
{
    private const string RoundScoresKey = "RoundHighScores";
    private const string QuestionScoresKey = "QuestionHighScores";
    private const int MaxScores = 5;

    public static int CurrentScore { get; private set; }

    public static void ResetScore()
    {
        CurrentScore = 0;
    }

    public static void AddScore(int points)
    {
        CurrentScore += points;
    }

    public static void SaveScore(string playerName, ScoreType type)
    {
        string key = (type == ScoreType.Round) ? RoundScoresKey : QuestionScoresKey;
        List<ScoreEntry> highScores = LoadScores(type);

        ScoreEntry newEntry = new ScoreEntry(playerName, CurrentScore);
        highScores.Add(newEntry);

        highScores.Sort((a, b) => b.score.CompareTo(a.score));
        if (highScores.Count > MaxScores)
            highScores.RemoveAt(highScores.Count - 1);

        ScoreList wrapper = new ScoreList(highScores);
        string json = JsonUtility.ToJson(wrapper, true);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

    }

    public static List<ScoreEntry> LoadScores(ScoreType type)
    {
        string key = (type == ScoreType.Round) ? RoundScoresKey : QuestionScoresKey;

        if (!PlayerPrefs.HasKey(key))
            return new List<ScoreEntry>();

        string json = PlayerPrefs.GetString(key);
        ScoreList wrapper = JsonUtility.FromJson<ScoreList>(json);

        if (wrapper == null || wrapper.scores == null)
            return new List<ScoreEntry>();

        return wrapper.scores;
    }
}

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;

    public ScoreEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}

[System.Serializable]
public class ScoreList
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();

    public ScoreList(List<ScoreEntry> scores)
    {
        this.scores = scores;
    }
}
