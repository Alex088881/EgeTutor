using EgeTutor.API.Middleware;
using EgeTutor.Application.Interfaces;
using EgeTutor.Application.Services;
using EgeTutor.Core.Interfaces;
using EgeTutor.Persistence.Data;
using EgeTutor.Persistence.Repisitories;
using EgeTutor.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EgeTutor.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();

            // Регистрация сервисов
            builder.Services.AddScoped<DbInitializer>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Обработка исключений
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // JWT
            builder.Services.AddScoped<ITokenService>(sp =>
                new TokenService(builder.Configuration["Jwt:Secret"]!, builder.Configuration["Jwt:Issuer"]!));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                    {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = false, // Для простоты
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
                    };
            });

            builder.Services.AddAuthorization();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Используй PostgreSQL (Npgsql) или другую БД
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

                // Для отладки: логируем SQL-запросы в консоль (убери в продакшене!)
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging(); // Только для разработки!
            });



            var app = builder.Build();
            app.UseExceptionHandler();
            app.UseMiddleware<RequestLoggingMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var initializer = services.GetRequiredService<DbInitializer>();
                    await initializer.InitializeAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            app.UseRouting();
            app.UseCors("AllowReactApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();



            app.Run();
        }
    }
}
