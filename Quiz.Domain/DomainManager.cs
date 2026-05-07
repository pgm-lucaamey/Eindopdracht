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
        public IEnumerable<Topic> GetAllTopics()
        {
            return _repository.GetAllTopics();
        }

        public void CreateTest(string name, int count, int topicId)
        {
            Test test = new Test(name, count, topicId);
            test.Id = _repository.AddTest(test);

            IEnumerable<Question> questions = _repository.GetRandomQuestion(topicId, count);

            foreach (Question question in questions)
            {
                _repository.AddTestQuestion(test.Id, question.Id);
            }
        }
    }
}