using EgeTutor.Core.Enums;
using EgeTutor.Core.Models;

namespace EgeTutor.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterUserAsync(string email, string password, string? firstName, string? lastName, string role = "Student");
        Task<string?> LoginUserAsync(string email, string password);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
