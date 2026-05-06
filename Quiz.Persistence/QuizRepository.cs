using Microsoft.Data.SqlClient;
using Quiz.Domain;
using Quiz.Domain.Interfaces;

namespace Quiz.Persistence
{
    public class QuizRepository : IQuizRepository
    {
        private string _connectionString;

        public QuizRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddAnswer(Answer answer, int questionId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, Label) 
                       VALUES (@QuestionId, @AnswerText, @IsCorrect, @Label); 
                       SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                    cmd.Parameters.AddWithValue("@AnswerText", answer.AnswerText);
                    cmd.Parameters.AddWithValue("@IsCorrect", answer.IsCorrect);
                    cmd.Parameters.AddWithValue("@Label", answer.Label.ToString());

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int AddQuestion(Question question)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Questions (TopicId, QuestionText, IsAvailable) 
                       VALUES (@TopicId, @QuestionText, @IsAvailable); 
                       SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TopicId", question.Topic.Id);
                    cmd.Parameters.AddWithValue("@QuestionText", question.QuestionText);
                    cmd.Parameters.AddWithValue("@IsAvailable", question.IsAvailable);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int AddTest(Test test)
        {
            throw new NotImplementedException();
        }

        public int AddTopic(Topic topic)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                
                string sql = "INSERT INTO Topics (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@Name", topic.Name);

                    
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
       
        public IEnumerable<Answer> GetAllAnswersByQuestion(int questionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> GetAllQuestionsByTopic(int topicId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Topic> GetAllTopics()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> GetRandomQuestion(int topicId, int count)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestionAvailability(int questionId, bool isAvailable)
        {
            throw new NotImplementedException();
        }
    }
}
