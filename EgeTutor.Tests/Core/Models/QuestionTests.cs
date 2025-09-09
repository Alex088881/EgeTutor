

using EgeTutor.Core.Models;

namespace EgeTutor.Tests.Core.Models
{
    public class QuestionTests
    {
        [Fact]
        public void Constructor_ValidData_CreatesQuestion()
        {
            // Act
            var question = new Question("Test question", "Test answer", 1);

            //Assert
            Assert.Equal("Test question",question.Text);
            Assert.Equal("Test answer", question.CorrectAnswer);
            Assert.Equal(1, question.TopicId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_EmptyText_ThrowsArgumentException(string text)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(()=>
                new Question(text, "Test answer", 1));
        }

        [Fact]
        public void UpdateText_ValidText_UpdatesProperty()
        {
            // Arrange
            var question = new Question("Old text", "Test answer", 1);

            // Act
            question.UpdateText("New text");

            // Assert
            Assert.Equal("New text", question.Text);
        }

        [Fact]
        public void ChangeTopic_ValidTopicId_UpdatesTopicId()
        {
            // Arrange
            var question = new Question("Test question", "Test answer", 1);

            // Act
            question.ChangeTopic(2);

            // Assert
            Assert.Equal(2, question.TopicId);
        }


    }
}
