using Microsoft.AspNetCore.Identity;
using IPasswordHasher = EgeTutor.Core.Interfaces.IPasswordHasher;

namespace EgeTutor.Services.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var result = _hasher.VerifyHashedPassword(null!, passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
