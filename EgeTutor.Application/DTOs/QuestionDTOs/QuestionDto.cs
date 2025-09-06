namespace EgeTutor.Application.DTOs
{
    public record QuestionDto(
        int Id,
        string Text,
        string CorrectAnswer,
        string? ImageUrl,
        int TopicId,
        string? AnswerUrl
    );
}
