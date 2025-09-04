using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;

namespace EgeTutor.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
           return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<string?> LoginUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            // Проверяем пароль
            var isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);
            if (!isPasswordValid) return null;

            // Генерируем JWT-токен
            return _tokenService.GenerateJwtToken(user);
        }

        public async Task<User?> RegisterUserAsync(string email, string password, string? firstName, string? lastName, string role = "Student")
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null) return null;

            // Хэшируем пароль
            var passwordHash = _passwordHasher.HashPassword(password);

            // Создаём пользователя
            var newUser = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Role = role
            };

            // Сохраняем в БД
            await _userRepository.AddAsync(newUser);
            return newUser;
        }
    }
}
