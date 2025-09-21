using EgeTutor.Core.Interfaces;
using EgeTutor.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EgeTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        public class RegisterRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required, MinLength(6)]
            public string Password { get; set; } = string.Empty;

            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        public class LoginRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await authService.RegisterUserAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName
                );
            if(user is null)
            {
                return BadRequest(new { message = "User with this email already exists." });
            }
            return Ok(new {message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await authService.LoginUserAsync(request.Email, request.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Возвращаем JWT токен клиенту
            return Ok(new { token = token });
        }

    }
}
