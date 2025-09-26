using System;

[Serializable]
public class QuestionData
{
    public string question;
    public string[] options; // 4 respuestas
    public int correctIndex; // índice de la respuesta correcta
}

[Serializable]
public class CategoryData
{
    public string category;
    public QuestionData[] questions;
}

[Serializable]
public class TriviaData
{
    public CategoryData[] categories;
}
