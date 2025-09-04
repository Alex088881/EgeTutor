using EgeTutor.Core.Models;

namespace EgeTutor.Core.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>?> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Question question, CancellationToken cancellationToken = default);
        Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task  Update(Question question, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Question>?> GetAllQuestionsOfTopicAsinc(int idTopic, CancellationToken cancellationToken = default);
    }
}
