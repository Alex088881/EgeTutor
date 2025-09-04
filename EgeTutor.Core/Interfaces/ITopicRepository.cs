using EgeTutor.Core.Models;

namespace EgeTutor.Core.Interfaces
{
    public interface ITopicRepository
    {
        Task<List<Topic>?> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Topic topic, CancellationToken cancellationToken = default);
        Task<Topic?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task Update(Topic topic, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
