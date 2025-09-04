using EgeTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EgeTutor.Persistence.Data.Configurations
{
    internal class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasKey(ua => ua.Id); // Явно указываем ключ

            // Настройка связи с Question
            builder.HasOne(ua => ua.Question) // У UserAnswer есть навигационное свойство Question
                .WithMany() // У Question много UserAnswer
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // Если удаляют вопрос, удаляются и ответы на него

            // Настройка индекса для часто используемых запросов
            // Например, мы часто будем искать ответы конкретного пользователя
            builder.HasIndex(ua => ua.UserId);
            builder.HasIndex(ua => ua.QuestionId);
        }
    }
}
