namespace EgeTutor.Core.Models
{
    public class UserAnswer
    {
        public int Id { get; private set; }
        public string UserId { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string UserProvidedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public DateTime AnsweredOn { get; set; } = DateTime.UtcNow;
    }
}
