

using EgeTutor.Core.Enums;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using EgeTutor.Services.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace EgeTutor.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockTokenService.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_NewUser_ReturnsUser()
        {
            // Arrange
            var passwordHash = "hashed_password";
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = passwordHash,
                FirstName = "Test",
                LastName = "User",
                Role = Roles.Student.ToString(),
            };

            _mockUserRepository.Setup(x => x.GetByEmailAsync("test@example.com"))
                .ReturnsAsync((User?)null);
            _mockPasswordHasher.Setup(x => x.HashPassword("password123"))
                .Returns(passwordHash);
            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>()));

            // Act
            var result = await _authService.RegisterUserAsync(
                "test@example.com", "password123", "Test", "User");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal(passwordHash, result.PasswordHash);
        }

        [Fact]
        public async Task RegisterUserAsync_ExistingUser_ReturnsNull()
        {
            // Arrange
            var existingUser = new User { Email = "test@example.com" };
            _mockUserRepository.Setup(x => x.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _authService.RegisterUserAsync(
                "test@example.com", "password123", "Test", "User");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginUserAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = "hashed_password",
            };

            var token = "jwt_token";

            _mockUserRepository.Setup(x=>x.GetByEmailAsync("test@example.com"))
                .ReturnsAsync((User?)user);

            _mockPasswordHasher.Setup(x => x.VerifyPassword("password123", "hashed_password"))
                .Returns(true);

            _mockTokenService.Setup(x => x.GenerateJwtToken(user))
                .Returns(token);

            // Act
            var result = await _authService.LoginUserAsync("test@example.com", "password123");

            // Assert
            Assert.Equal(token, result);
        }
        [Fact]
        public async Task LoginUserAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new User { Email = "test@example.com", PasswordHash = "hashed_password" };
            _mockUserRepository.Setup(x => x.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(user);

            _mockPasswordHasher.Setup(x => x.VerifyPassword("wrong_password", "hashed_password"))
                .Returns(false);

            // Act
            var result = await _authService.LoginUserAsync("test@example.com", "wrong_password");

            // Assert
            Assert.Null(result);
        }

    }

}
