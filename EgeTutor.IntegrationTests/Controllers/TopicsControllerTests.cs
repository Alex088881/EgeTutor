

using EgeTutor.IntegrationTests.Fixtures;
using EgeTutor.Persistence.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EgeTutor.IntegrationTests.Controllers
{
    public class TopicsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private string _authToken = string.Empty;

        public TopicsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        // IAsyncLifetime методы для инициализации и очистки
        public async Task InitializeAsync()
        {
            // Инициализируем базу данных
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Очищаем и создаём базу
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            // Регистрируем и логиним пользователя
            await RegisterAndLoginAdmin();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        private async Task RegisterAndLoginAdmin()
        {
            var registerRequest = new
            {
                Email = "admin@test.com",
                Password = "Admin123!",
                FirstName = "Admin",
                LastName = "Test"
            };

            await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

            var loginRequest = new
            {
                Email = "admin@test.com",
                Password = "Admin123!"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            _authToken = result!.Token;

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authToken);
        }

        [Fact]
        public async Task CreateTopic_AsAdmin_ReturnsCreated()
        {
            // Arrange
            var createTopicRequest = new
            {
                Name = "Test Topic",
                Description = "Test Description"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/topics", createTopicRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
