using System;
using System.Collections.Generic;
using System.IO;
using Quiz.Domain;
using Quiz.Persistence;

namespace Quiz.Persistence
{
    public class TxtParser
    {
        public List<Question> Parse(string filePath, Topic topic)
        {
            string[] lines = File.ReadAllLines(filePath);

            bool isFormatCorrect = lines.Any(l => l.StartsWith("Correct:"));

            if (isFormatCorrect)
                return ParseFormatCorrect(lines, topic);
            else
                return ParseFormatAntwoorden(lines, topic);
        }

        private List<Question> ParseFormatCorrect(string[] lines, Topic topic)
        {
            List<Question> questions = new List<Question>();
            Question currentQuestion = null;
            Dictionary<char, string> temporaryAnswers = new Dictionary<char, string>();

            foreach (string line in lines)
            {
                string trimmed = line.Trim();

                if (trimmed.Length > 0 && char.IsDigit(trimmed[0]))
                {
                    currentQuestion = new Question(trimmed, topic);
                    questions.Add(currentQuestion);
                    temporaryAnswers.Clear();
                }
                else if (trimmed.StartsWith("A.") || trimmed.StartsWith("B.") ||
                         trimmed.StartsWith("C.") || trimmed.StartsWith("D."))
                {
                    char label = trimmed[0];
                    string text = trimmed.Substring(2).Trim();
                    temporaryAnswers[label] = text;
                }
                else if (trimmed.StartsWith("Correct:"))
                {
                    char correct = trimmed.Replace("Correct:", "").Trim()[0];

                    foreach (var kv in temporaryAnswers)
                    {
                        bool isCorrect = (kv.Key == correct);
                        currentQuestion.AddAnswer(new Answer(kv.Value, isCorrect, kv.Key));
                    }
                }
            }
            return questions;
        }

        private List<Question> ParseFormatAntwoorden(string[] lines, Topic topic)
        {
            List<Question> questions = new List<Question>();
            Question currentQuestion = null;
            
            Dictionary<int, Dictionary<char, string>> allAnswers = new Dictionary<int, Dictionary<char, string>>();
            int currentQuestionNumber = 0;
            bool inAnswerSection = false;
            int answerIndex = 0;

            foreach (string line in lines)
            {
                string trimmed = line.Trim();

                if (inAnswerSection)
                {
                    if (trimmed.Length == 1 && char.IsLetter(trimmed[0]))
                    {
                        char correct = char.ToUpper(trimmed[0]);
                        Question q = questions[answerIndex];

                        foreach (var kv in allAnswers[answerIndex + 1])
                        {
                            bool isCorrect = (kv.Key == correct);
                            q.AddAnswer(new Answer(kv.Value, isCorrect, kv.Key));
                        }
                        answerIndex++;
                    }
                }
                else if (trimmed == "Antwoorden")
                {
                    inAnswerSection = true;
                }
                else if (trimmed.Length > 0 && char.IsDigit(trimmed[0]))
                {
                    currentQuestionNumber = int.Parse(trimmed.Split('.')[0]);
                    currentQuestion = new Question(trimmed, topic);
                    questions.Add(currentQuestion);
                    allAnswers[currentQuestionNumber] = new Dictionary<char, string>();
                }
                else if (trimmed.StartsWith("A.") || trimmed.StartsWith("B.") ||
                         trimmed.StartsWith("C.") || trimmed.StartsWith("D."))
                {
                    char label = trimmed[0];
                    string text = trimmed.Substring(2).Trim();
                    allAnswers[currentQuestionNumber][label] = text;
                }
            }
            return questions;
        }
    }
}