using EgeTutor.Application.Services;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgeTutor.Tests.Application.Services
{
    public class TopicServiceTests
    {
        private readonly Mock<ITopicRepository> _mockTopicRepository;
        private readonly TopicService _topicService;

        public TopicServiceTests()
        {
            _mockTopicRepository = new Mock<ITopicRepository>();
            _topicService = new TopicService(_mockTopicRepository.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsTopicDto()
        {
            // Arrange
            var topic = new Topic("Test Topic", "Test Description");

            Topic? capturedTopic = null;

            _mockTopicRepository.Setup(x => x.AddAsync(It.IsAny<Topic>(), It.IsAny<CancellationToken>()))
                .Callback<Topic, CancellationToken>((t,ct)=> capturedTopic =t )
                .Returns(Task.CompletedTask);

            // Act
            var result = await _topicService.CreateAsync("Test Topic", "Test Description");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Topic", result.Name);
            Assert.Equal("Test Description", result.Description);

            Assert.NotNull(capturedTopic);
            Assert.Equal("Test Topic", capturedTopic.Name);
        }
    }
}
