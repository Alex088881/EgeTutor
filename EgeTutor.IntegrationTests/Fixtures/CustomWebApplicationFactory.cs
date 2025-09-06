
using EgeTutor.API;
using EgeTutor.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EgeTutor.IntegrationTests.Fixtures
{
    public class CustomWebApplicationFactory:WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                // Удаляем регистрацию реального DbContext
                var descroptor = services.SingleOrDefault(d=>d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descroptor != null)
                {
                    services.Remove(descroptor);
                }
                // Добавляем InMemory базу данных для тестов
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Создаём scope для получения сервисов
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var scopedSevices = scope.ServiceProvider;
                var db = scopedSevices.GetRequiredService<ApplicationDbContext>();

                // Гарантируем, что база данных создана
                db.Database.EnsureCreated();
            });
        }
    }
}
