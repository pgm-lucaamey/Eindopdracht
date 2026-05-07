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

        public void AddQuestion(string questionText, int topicId, string a, string b, string c, string d, char correct)
        {
            Topic topic = new Topic("temp");
            topic.Id = topicId;

            Question question = new Question(questionText, topic);
            question.AddAnswer(new Answer(a, correct == 'A', 'A'));
            question.AddAnswer(new Answer(b, correct == 'B', 'B'));
            question.AddAnswer(new Answer(c, correct == 'C', 'C'));
            question.AddAnswer(new Answer(d, correct == 'D', 'D'));

            question.Id = _repository.AddQuestion(question);

            foreach (Answer answer in question.Answers)
            {
                _repository.AddAnswer(answer, question.Id);
            }
        }

        public IEnumerable<Question> GetQuestionsByTopic(int topicId)
        {
            return _repository.GetAllQuestionsByTopic(topicId);
        }

        public void DisableQuestion(int questionId)
        {
            _repository.UpdateQuestionAvailability(questionId, false);
        }
        
        public int AddTopic(Topic topic)
        {
            Topic existing = _repository.GetTopicByName(topic.Name);
            if (existing != null)
            {
                Console.WriteLine($"Topic '{topic.Name}' bestaat al — import overgeslagen.");
                return -1;
            }
            topic.Id = _repository.AddTopic(topic);
            return topic.Id;
        }
        public IEnumerable<Test> GetAllTests()
        {
            return _repository.GetAllTests();
        }

        public List<Question> GetQuestionsForTest(int testId)
        {
            return _repository.GetQuestionsForTest(testId);
        }

        public void SaveResult(int testId, int score)
        {
            _repository.SaveResult(testId, score);
        }
    }
}