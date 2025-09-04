using EgeTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EgeTutor.Persistence.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256); // Ограничение длины для varchar в БД

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasMaxLength(100);

            // Создание уникального индекса для поля Email
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Настройка связи "Один ко многим" (У одного User много UserAnswer)
            builder.HasMany(u => u.UserAnswers)
                .WithOne() // У UserAnswer нет навигационного свойства обратно к User
                .HasForeignKey(ua => ua.UserId) // Внешний ключ в таблице UserAnswers
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
