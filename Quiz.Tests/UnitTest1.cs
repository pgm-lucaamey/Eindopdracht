using Quiz.Domain;
using Quiz.Domain.Exceptions;
using Xunit;

namespace Quiz.Tests
{
    public class UnitTest1
    {
        // Topic tests
        [Fact]
        public void Topic_ValidName_ShouldCreateTopic()
        {
            Topic topic = new Topic("Geo1");
            Assert.Equal("Geo1", topic.Name);
        }

        [Fact]
        public void Topic_EmptyName_ShouldThrowException()
        {
            Assert.Throws<InvalidQuestionException>(() => new Topic(""));
        }

        [Fact]
        public void Topic_NullName_ShouldThrowException()
        {
            Assert.Throws<InvalidQuestionException>(() => new Topic(null));
        }
        // Answer tests
        [Fact]
        public void Answer_ValidData_ShouldCreateAnswer()
        {
            Answer answer = new Answer("Brussel", true, 'A');
            Assert.Equal("Brussel", answer.AnswerText);
            Assert.True(answer.IsCorrect);
            Assert.Equal('A', answer.Label);
        }

        [Fact]
        public void Answer_EmptyText_ShouldThrowException()
        {
            Assert.Throws<InvalidQuestionException>(() => new Answer("", true, 'A'));
        }

        [Fact]
        public void Answer_NullText_ShouldThrowException()
        {
            Assert.Throws<InvalidQuestionException>(() => new Answer(null, true, 'A'));
        }

        // Question tests
        [Fact]
        public void Question_ValidData_ShouldCreateQuestion()
        {
            Topic topic = new Topic("Geo1");
            Question question = new Question("Wat is de hoofdstad van België?", topic);
            Assert.Equal("Wat is de hoofdstad van België?", question.QuestionText);
            Assert.True(question.IsAvailable);
        }

        [Fact]
        public void Question_EmptyText_ShouldThrowException()
        {
            Topic topic = new Topic("Geo1");
            Assert.Throws<InvalidQuestionException>(() => new Question("", topic));
        }

        [Fact]
        public void Question_NullTopic_ShouldThrowException()
        {
            Assert.Throws<InvalidQuestionException>(() => new Question("Wat is de hoofdstad?", null));
        }

        [Fact]
        public void Question_GetCorrectAnswer_ShouldReturnCorrectAnswer()
        {
            Topic topic = new Topic("Geo1");
            Question question = new Question("Wat is de hoofdstad van België?", topic);
            question.AddAnswer(new Answer("Gent", false, 'A'));
            question.AddAnswer(new Answer("Brussel", true, 'B'));
            question.AddAnswer(new Answer("Antwerpen", false, 'C'));
            question.AddAnswer(new Answer("Luik", false, 'D'));

            Answer correct = question.GetCorrectAnswer();
            Assert.Equal('B', correct.Label);
            Assert.Equal("Brussel", correct.AnswerText);
        }

        [Fact]
        public void Question_IsAvailable_DefaultShouldBeTrue()
        {
            Topic topic = new Topic("Geo1");
            Question question = new Question("Wat is de hoofdstad van België?", topic);
            Assert.True(question.IsAvailable);
        }
    }
}   