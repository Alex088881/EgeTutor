using EgeTutor.Core.Enums;

namespace EgeTutor.Core.Models
{
    public class User:BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; } = Roles.Student.ToString();
        public List<UserAnswer> UserAnswers { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
