

using EgeTutor.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace EgeTutor.IntegrationTests.Controllers
{
    public class AuthControllerTests:IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Register_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var registerRequest = new
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.NotNull(content);

        }
        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new
            {
                Email = "duplicate@example.com",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User"
            };

            // Первая регистрация
            await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            // Вторая попытка с тем же email
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var registerRequest = new
            {
                Email = "login@example.com",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User"
            };

            // Сначала регистрируем пользователя
            await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            var loginRequest = new
            {
                Email = "login@example.com",
                Password = "Password123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.NotNull(content);
            Assert.NotNull(content!.Token);
        }
    }
}
