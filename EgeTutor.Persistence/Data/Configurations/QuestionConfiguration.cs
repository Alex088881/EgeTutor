using EgeTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EgeTutor.Persistence.Data.Configurations
{
    internal class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.CorrectAnswer)
            .IsRequired()
            .HasMaxLength(256); // Ограничение длины для varchar в БД

            builder.Property(q => q.Text)
                .IsRequired();

            builder.HasOne(q => q.Topic)
                .WithMany() // У UserAnswer нет навигационного свойства обратно к User
                .HasForeignKey(q => q.TopicId) // Внешний ключ в таблице
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
