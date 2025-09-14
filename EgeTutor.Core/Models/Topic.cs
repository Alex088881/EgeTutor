namespace EgeTutor.Core.Models
{
    public class Topic
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public List<Question> Questions { get;private set; } = [];
        protected Topic() { }

        public Topic(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (name.Length > 100)
                throw new ArgumentException("Name is too long", nameof(name));

            if (description?.Length > 500)
                throw new ArgumentException("Description is too long", nameof(description));

            Name = name;
            Description = description;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty", nameof(newName));

            if (newName.Length > 100)
                throw new ArgumentException("Name is too long", nameof(newName));

            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty", nameof(newDescription));

            if (newDescription?.Length > 500)
                throw new ArgumentException("Description is too long", nameof(newDescription));

            Description = newDescription;
        }

        public void AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);
            Questions.Add(question);
        }
        public void RemoveQuestion(int questionId)
        {
            var questionToRemove = Questions.FirstOrDefault(q => q.Id == questionId) 
                ?? throw new ArgumentException("Question not found in this topic", nameof(questionId));

            Questions.Remove(questionToRemove);
        }
    }
}
