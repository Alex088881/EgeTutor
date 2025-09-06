

namespace EgeTutor.Core.Exceptions
{
    public class ValidationException:Exception
    {
        public ValidationException(string message) : base(message) { }
        public ValidationException(IEnumerable<string> failures)
            : base("Validation errors occurred")
        {
            Errors = failures;
        }
        public IEnumerable<string>? Errors { get; }
    }
}
