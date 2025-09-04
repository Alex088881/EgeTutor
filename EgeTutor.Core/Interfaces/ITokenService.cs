using EgeTutor.Core.Models;

namespace EgeTutor.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        int? ValidateJwtToken(string token);
    }
}
