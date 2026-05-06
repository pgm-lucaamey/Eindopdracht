using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain
{
    public class Question
    {
        private int _id;
        private string _questionText;
        private Topic _topic;
        private List<Answer> _answers;
        private bool _isAvailable;

        public int Id { get { return _id; } set { _id = value; } }
        public string QuestionText { get { return _questionText; } }

        public Topic Topic { get { return _topic; } }

        public bool IsAvailable {  get { return _isAvailable; } set { _isAvailable =  value; } }

        public List<Answer> Answers { get { return _answers; } }

        public Question(string questionText, Topic topic)
        {
            if(string.IsNullOrEmpty(questionText))
            {
                throw new ArgumentException("Question text cannot be null or empty.");
            }

            if(topic == null)
            {
                throw new ArgumentException("Topic cannot be null.");
            }
            _questionText = questionText;
            _topic = topic;
            _isAvailable = true;
            _answers = new List<Answer>();
        }

        public void AddAnswer(Answer answer)
        {
            _answers.Add(answer);
        }

        public Answer GetCorrectAnswer()
        {
            foreach (Answer answer in _answers)
            {
                if (answer.IsCorrect)
                    return answer;
            }
            return null;
        }
      
    }
}
