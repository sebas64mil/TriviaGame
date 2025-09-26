using System.Collections.Generic;
using UnityEngine;

public static class TriviaLoader
{
    public static TriviaData LoadQuestions()
    {
        TriviaData triviaData = new TriviaData
        {
            categories = new CategoryData[]
            {
                // ---------------- Historia
                new CategoryData
                {
                    category = "Historia",
                    questions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "¿Quién descubrió América?",
                            options = new string[] { "Colón", "Einstein", "Newton", "Tesla" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿En qué año terminó la Segunda Guerra Mundial?",
                            options = new string[] { "1945", "1939", "1918", "1965" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Quién fue el primer presidente de Estados Unidos?",
                            options = new string[] { "Washington", "Lincoln", "Jefferson", "Roosevelt" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿En qué año cayó el Imperio Romano de Occidente?",
                            options = new string[] { "476 d.C.", "1492", "1066", "800 d.C." },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Qué civilización construyó las pirámides de Giza?",
                            options = new string[] { "Egipcia", "Maya", "Azteca", "China" },
                            correctIndex = 0
                        }
                    }
                },

                // ---------------- Ciencia
                new CategoryData
                {
                    category = "Ciencia",
                    questions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "¿Cuál es el planeta más cercano al Sol?",
                            options = new string[] { "Mercurio", "Venus", "Tierra", "Marte" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Qué partícula tiene carga positiva?",
                            options = new string[] { "Protón", "Neutrón", "Electrón", "Fotón" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Cuál es el gas más abundante en la atmósfera terrestre?",
                            options = new string[] { "Nitrógeno", "Oxígeno", "Dióxido de carbono", "Hidrógeno" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Qué científico propuso la teoría de la relatividad?",
                            options = new string[] { "Einstein", "Newton", "Galileo", "Bohr" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Cuál es el metal más abundante en la corteza terrestre?",
                            options = new string[] { "Aluminio", "Hierro", "Cobre", "Plata" },
                            correctIndex = 0
                        }
                    }
                },

                // ---------------- Geografía
                new CategoryData
                {
                    category = "Geografía",
                    questions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "¿Cuál es el río más largo del mundo?",
                            options = new string[] { "Nilo", "Amazonas", "Yangtsé", "Misisipi" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Cuál es la montaña más alta del mundo?",
                            options = new string[] { "Everest", "K2", "Kangchenjunga", "Makalu" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Cuál es la capital de Australia?",
                            options = new string[] { "Canberra", "Sídney", "Melbourne", "Brisbane" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿En qué continente está Botswana?",
                            options = new string[] { "África", "Asia", "Oceanía", "América" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Cuál es el país más poblado del mundo?",
                            options = new string[] { "India", "China", "Estados Unidos", "Indonesia" },
                            correctIndex = 1
                        }
                    }
                },

                // ---------------- Deportes
                new CategoryData
                {
                    category = "Deportes",
                    questions = new QuestionData[]
                    {
                        new QuestionData
                        {
                            question = "¿Cuántos jugadores hay en un equipo de fútbol?",
                            options = new string[] { "11", "9", "10", "12" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿En qué deporte se utiliza un 'smash'?",
                            options = new string[] { "Bádminton", "Tenis", "Voleibol", "Ping-pong" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Cuántos puntos vale un touchdown en fútbol americano?",
                            options = new string[] { "6", "3", "7", "1" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿Qué país ganó la Copa Mundial de Fútbol 2018?",
                            options = new string[] { "Francia", "Brasil", "Alemania", "Argentina" },
                            correctIndex = 0
                        },
                        new QuestionData
                        {
                            question = "¿En qué deporte se destaca Michael Jordan?",
                            options = new string[] { "Baloncesto", "Béisbol", "Fútbol", "Hockey" },
                            correctIndex = 0
                        }
                    }
                }
            }
        };

        //  Mezclar respuestas y actualizar correctIndex
        ShuffleAnswers(triviaData);

        return triviaData;
    }

    private static void ShuffleAnswers(TriviaData triviaData)
    {
        foreach (var category in triviaData.categories)
        {
            foreach (var question in category.questions)
            {
                // Guardamos la respuesta correcta original
                string correctAnswer = question.options[question.correctIndex];

                // Convertir a lista para barajar
                List<string> shuffled = new List<string>(question.options);
                for (int i = 0; i < shuffled.Count; i++)
                {
                    int rnd = Random.Range(i, shuffled.Count);
                    (shuffled[i], shuffled[rnd]) = (shuffled[rnd], shuffled[i]);
                }

                // Reemplazar opciones y recalcular índice correcto
                question.options = shuffled.ToArray();
                question.correctIndex = System.Array.IndexOf(question.options, correctAnswer);
            }
        }
    }
}
