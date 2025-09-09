using EgeTutor.API.Controllers;
using EgeTutor.Application.DTOs;
using EgeTutor.Application.Interfaces;
using EgeTutor.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace EgeTutor.Tests.API.Controllers
{
    public class QuestionsControllerTests
    {
        private readonly Mock<IQuestionService> _mockQuestionService;
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            _mockQuestionService = new Mock<IQuestionService>();
            _controller = new QuestionsController(
                    Mock.Of<ILogger<QuestionsController>>(),
                    _mockQuestionService.Object
                );
            // Mock user context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, Roles.Admin.ToString())
            }, "mock"));

            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetQuestion_Exists_ReturnsOk()
        {
            // Arrange
            var questionDto = new QuestionDto(1, "Test", "Answer", null, 1, null);
            _mockQuestionService.Setup(x=>x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionDto);

            //Act
            var result = await _controller.GetQuestion(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuestion = Assert.IsType<QuestionDto>(okResult.Value);
            Assert.Equal(1, returnedQuestion.Id);

        }

        [Fact]
        public async Task CreateQuestion_ValidData_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateQuestionDto
            {
                Text = "Test question",
                CorrectAnswer = "Test answer",
                TopicId = 1
            };

            var questionDto = new QuestionDto(1, "Test question", "Test answer", null, 1, null);
            _mockQuestionService.Setup(x => x.CreateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionDto);

            // Act
            var result = await _controller.CreateQuestion(createDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(QuestionsController.CreateQuestion), createdAtResult.ActionName);
        }



    }
}
