using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain
{
    public class Answer
    {
        private string _answerText;
        private int _id;
        private bool _isCorrect;
        private char _label;

        public string AnswerText
        {
            get { return _answerText; }
        }

        public int Id
        {
            get { return _id; }
        }

        public bool IsCorrect
        {
            get { return _isCorrect; }
        }
        public char Label
        {
            get { return _label; }
        }

        public Answer(string answerText, bool isCorrect, char label)
        {
            if (string.IsNullOrWhiteSpace(answerText))
            {
                throw new ArgumentException("Answer text cannot be null or empty.");
            }
            
            _answerText = answerText;
            _isCorrect = isCorrect;
            _label = label;

        }
    }
}
