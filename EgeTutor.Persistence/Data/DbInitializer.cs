using EgeTutor.Core.Enums;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EgeTutor.Persistence.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext context;
        private readonly IPasswordHasher passwordHasher;

        public DbInitializer(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        public async Task InitializeAsync()
        {
            if (context.Users.Any()) return;

            var adminUser = new User
            {
                Email = "admin@mail.com",
                PasswordHash = passwordHasher.HashPassword("admin"),
                FirstName = "Admin",
                LastName = "System",
                Role = Roles.Admin.ToString()
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
