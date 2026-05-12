using Quiz.Domain.DTO;
using Quiz.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quiz.Domain 
{
    public class DomainManager
    {
        private IQuizRepository _repository;

        public DomainManager(IQuizRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Topic> GetAllTopics()
        {
            return _repository.GetAllTopics();
        }
        public void ImportFromFile(Topic topic, List<Question> questions)
        {
            _repository.ImportQuestions(topic, questions);
        }

        public bool TopicExists(string name)
        {
            return _repository.GetTopicByName(name) != null;
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
        public List<QuestionDTO> GetQuestionsForTestDTO(int testId)
        {
            return _repository.GetQuestionsForTestDTO(testId);
        }

        public IEnumerable<QuestionDTO> GetQuestionsByTopicDTO(int topicId)
        {
            return _repository.GetQuestionsByTopicDTO(topicId);
        }

    }
}