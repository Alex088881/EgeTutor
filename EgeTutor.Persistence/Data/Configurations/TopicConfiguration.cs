using EgeTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EgeTutor.Persistence.Data.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(256); // Ограничение длины для varchar в БД

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(256);

            // Настройка связи "Один ко многим" (У одного User много UserAnswer)
            builder.HasMany(t => t.Questions)
                .WithOne() // У UserAnswer нет навигационного свойства обратно к User
                .HasForeignKey(q => q.TopicId) // Внешний ключ в таблице UserAnswers
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
