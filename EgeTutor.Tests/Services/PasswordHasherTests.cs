
using EgeTutor.Services.Services;

namespace EgeTutor.Tests.Services
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordHasherTests()
        {
            _passwordHasher = new PasswordHasher();
        }

        [Fact]
        public void HashPassword_ValidPassword_ReturnsHash()
        {
            // Arrange
            var password = "test_password";

            // Act
            var hash = _passwordHasher.HashPassword(password);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "test_password";
            var hash = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_WrongPassword_ReturnsFalse()
        {
            // Arrange
            var password = "test_password";
            var wrongPassword = "wrong_password";
            var hash = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(wrongPassword, hash);

            // Assert
            Assert.False(result);
        }
    }
}
