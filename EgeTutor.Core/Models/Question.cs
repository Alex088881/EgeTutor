namespace EgeTutor.Core.Models
{
    public class Question
    {
        public int Id { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string CorrectAnswer { get; private set; } = string.Empty;
        public string? ImageUrl { get; private set; }
        public int TopicId { get; private set; }
        public Topic Topic { get; private set; } = null!;

        public string? AnswerUrl { get; private set; }

        protected Question() { }

        public Question(string text, string correctAnswer, int topicId, string? imageUrl = null, string? answerUrl = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Question text cannot be empty", nameof(text));
            if (string.IsNullOrWhiteSpace(correctAnswer))
                throw new ArgumentException("Correct answer cannot be empty", nameof(correctAnswer));
            if (topicId <= 0)
                throw new ArgumentException("Topic ID must be positive", nameof(topicId));

            Text = text;
            CorrectAnswer = correctAnswer;
            TopicId = topicId;
            ImageUrl = imageUrl;
            AnswerUrl = answerUrl;
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                throw new ArgumentException("Question text cannot be empty", nameof(newText));

            Text = newText;
        }


        public void UpdateCorrectAnswer(string newAnswer)
        {
            if (string.IsNullOrWhiteSpace(newAnswer))
                throw new ArgumentException("Correct answer cannot be empty", nameof(newAnswer));

            CorrectAnswer = newAnswer;
        }

        public void UpdateImageUrl(string? newImageUrl)
        {
            ImageUrl = newImageUrl;
        }

        public void UpdateAnswerUrl(string? newAnswerUrl)
        {
            AnswerUrl = newAnswerUrl;
        }

        public void ChangeTopic(int newTopicId)
        {
            if (newTopicId <= 0)
                throw new ArgumentException("Topic ID must be positive", nameof(newTopicId));

            TopicId = newTopicId;
        }
    }
}
