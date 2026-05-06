using System;
using System.Collections.Generic;
using System.IO;
using Quiz.Domain.Interfaces;

namespace Quiz.Domain 
{
    public class DomainManager
    {
        private IQuizRepository _repository;

        public DomainManager(IQuizRepository repository)
        {
            _repository = repository;
        }

        public int AddTopic(Topic topic)
        {
            topic.Id = _repository.AddTopic(topic);
            return topic.Id;
        }

        public void ImportFromFile(List<Question> questions)
        {
            foreach (Question question in questions)
            {
                question.Id = _repository.AddQuestion(question);

                foreach (Answer answer in question.Answers)
                {
                    _repository.AddAnswer(answer, question.Id);
                }
            }
        }
    }
}