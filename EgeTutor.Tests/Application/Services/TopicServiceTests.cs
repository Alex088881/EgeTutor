using EgeTutor.Application.Services;
using EgeTutor.Core.Exceptions;
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
        [Fact]
        public async Task GetByIdAsync_TopicExists_ReturnsTopicDto()
        {
            // Arrange
            var topic = new Topic("Test Topic", "Test Description");

            _mockTopicRepository.Setup(x => x.GetByIdAsync(0, It.IsAny<CancellationToken>()))
                .ReturnsAsync(topic);

            // Act
            var result = await _topicService.GetByIdAsync(0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Equal("Test Topic", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_TopicNotExists_ThrowsNotFoundException()
        {
            // Arrange
            _mockTopicRepository.Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Topic?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _topicService.GetByIdAsync(999));
        }

        [Fact]
        public async Task Update_ValidData_UpdatesTopic()
        {
            // Arrange
            var existingTopic = new Topic("Old Name", "Old Description");

            _mockTopicRepository.Setup(x => x.GetByIdAsync(0, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingTopic);

            // Act
            await _topicService.Update(0, "New Name", "New Description");

            // Assert
            _mockTopicRepository.Verify(x => x.Update(It.Is<Topic>(t =>
                t.Name == "New Name" &&
                t.Description == "New Description"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
