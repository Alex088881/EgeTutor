
using EgeTutor.Application.DTOs;
using EgeTutor.Application.Services;
using EgeTutor.Core.Exceptions;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using Moq;

namespace EgeTutor.Tests.Application.Services
{
    public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _mockQuestionRepository;
        private readonly Mock<ITopicRepository> _mockTopicRepository;
        private readonly QuestionService _questionService;

        public QuestionServiceTests()
        {
            _mockQuestionRepository = new Mock<IQuestionRepository>();
            _mockTopicRepository = new Mock<ITopicRepository>();
            _questionService = new QuestionService(
                _mockQuestionRepository.Object,
                _mockTopicRepository.Object);
        }
        [Fact]
        public async Task CreateAsync_ValidData_ReturnsQuestionDto()
        {
            // Arrange
            var createDto = new CreateQuestionDto
            {
                Text = "Test question",
                CorrectAnswer = "Test answer",
                TopicId = 1
            };

            Question? capturedQuestion = null;

            _mockTopicRepository.Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockQuestionRepository.Setup(x => x.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                .Callback<Question, CancellationToken>((q, ct) => capturedQuestion = q)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _questionService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test question", result.Text);
            Assert.Equal("Test answer", result.CorrectAnswer);
            Assert.Equal(1, result.TopicId);

            // Проверяем, что вопрос был передан в репозиторий
            Assert.NotNull(capturedQuestion);
            Assert.Equal("Test question", capturedQuestion.Text);
        }
        [Fact]
        public async Task CreateAsync_TopicNotExists_ThrowsValidationException()
        {
            // Arrange
            var createDto = new CreateQuestionDto
            {
                Text = "Test question",
                CorrectAnswer = "Test answer",
                TopicId = 999
            };

            _mockTopicRepository.Setup(x => x.ExistsAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _questionService.CreateAsync(createDto));
        }

        [Fact]
        public async Task GetByIdAsync_QuestionExists_ReturnsQuestionDto()
        {
            // Arrange
            var question = new Question("Test question", "Test answer", 1);
            //question.GetType().GetProperty("Id")?.SetValue(question, 1);

            _mockQuestionRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            // Act
            var result = await _questionService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal("Test question", result.Text);
        }
    }
}
