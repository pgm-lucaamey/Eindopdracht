using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain.Interfaces
{
    public interface IQuizRepository
    {
        IEnumerable<Topic> GetAllTopics();
        int AddAnswer(Answer answer, int questionId);
        int AddTopic(Topic topic);
        IEnumerable<Question> GetAllQuestionsByTopic(int topicId);
        int AddQuestion(Question question);
        int AddTest(Test test);
        IEnumerable<Answer> GetAllAnswersByQuestion(int questionId);
        IEnumerable<Question>GetRandomQuestion(int topicId, int count);
        void UpdateQuestionAvailability(int questionId, bool isAvailable);
        void AddTestQuestion(int testId, int questionId);
        Topic GetTopicByName(string name);
    }

}
