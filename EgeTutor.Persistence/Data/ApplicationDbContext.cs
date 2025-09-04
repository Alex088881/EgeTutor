using EgeTutor.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EgeTutor.Persistence.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<UserAnswer> UserAnswers => Set<UserAnswer>();
    }
}
