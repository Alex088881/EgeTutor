using System.ComponentModel.DataAnnotations;

namespace EgeTutor.Application.DTOs
{
    public class CreateQuestionDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        public int TopicId { get; set; }

        public string? AnswerUrl { get; set; }
    }
}
