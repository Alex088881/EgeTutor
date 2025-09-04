using System.ComponentModel.DataAnnotations;

namespace EgeTutor.API.DTOs
{
    public class UpdateQuestionDto
    {
        [Required(ErrorMessage = "Text is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Text must be between 10 and 2000 characters")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "CorrectAnswer is required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "CorrectAnswer must be between 1 and 500 characters")]
        public string CorrectAnswer { get; set; } = string.Empty;

        [Url(ErrorMessage = "ImageUrl must be a valid URL")]
        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TopicId must be positive")]
        public int TopicId { get; set; }

        [Url(ErrorMessage = "AnswerUrl must be a valid URL")]
        public string? AnswerUrl { get; set; }
    }
}
