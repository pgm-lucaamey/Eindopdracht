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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Tests (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", test.Name);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
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
        public void AddTestQuestion(int testId, int questionId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO TestQuestions (TestId, QuestionId) VALUES (@TestId, @QuestionId)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TestId", testId);
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                    cmd.ExecuteNonQuery();
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
            List<Topic> topics = new List<Topic>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Name FROM Topics";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Topic topic = new Topic(reader.GetString(1));
                        topic.Id = reader.GetInt32(0);
                        topics.Add(topic);
                    }
                }
            }
            return topics;
        }

        public IEnumerable<Question> GetRandomQuestion(int topicId, int count)
        {
            List<Question> questions = new List<Question>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT TOP (@Count) Id, QuestionText 
                       FROM Questions 
                       WHERE TopicId = @TopicId AND IsAvailable = 1
                       ORDER BY NEWID()";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Count", count);
                    cmd.Parameters.AddWithValue("@TopicId", topicId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Topic topic = new Topic("temp");
                            topic.Id = topicId;
                            Question q = new Question(reader.GetString(1), topic);
                            q.Id = reader.GetInt32(0);
                            questions.Add(q);
                        }
                    }
                }
            }
            return questions;
        }

        public void UpdateQuestionAvailability(int questionId, bool isAvailable)
        {
            throw new NotImplementedException();
        }
    }
}
