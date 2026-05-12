using Moq;
using Quiz.Domain;
using Quiz.Domain.DTO;
using Quiz.Domain.Interfaces;
using Xunit;

namespace Quiz.Tests
{
    public class DomainManagerTests
    {
        private Mock<IQuizRepository> _mockRepository;
        private DomainManager _manager;

        public DomainManagerTests()
        {
            _mockRepository = new Mock<IQuizRepository>();
            _manager = new DomainManager(_mockRepository.Object);
        }

        [Fact]
        public void TopicExists_ExistingTopic_ShouldReturnTrue()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetTopicByName("Geo1"))
                .Returns(new Topic("Geo1"));

            // Act
            bool result = _manager.TopicExists("Geo1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TopicExists_NonExistingTopic_ShouldReturnFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetTopicByName("Geo1"))
                .Returns((Topic)null);

            // Act
            bool result = _manager.TopicExists("Geo1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddQuestion_CorrectAnswerA_ShouldMarkAAsCorrect()
        {
            // Arrange
            _mockRepository.Setup(r => r.AddQuestion(It.IsAny<Question>())).Returns(1);
            _mockRepository.Setup(r => r.AddAnswer(It.IsAny<Answer>(), It.IsAny<int>())).Returns(1);

            // Act
            _manager.AddQuestion("Wat is de hoofdstad?", 1, "Brussel", "Gent", "Antwerpen", "Luik", 'A');

            // Assert
            _mockRepository.Verify(r => r.AddAnswer(
                It.Is<Answer>(a => a.Label == 'A' && a.IsCorrect == true), It.IsAny<int>()), Times.Once);
        }
    }
}
