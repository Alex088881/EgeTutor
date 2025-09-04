using EgeTutor.Application.DTOs.TopicDTOs;
using EgeTutor.Core.Models;

namespace EgeTutor.Application.Interfaces
{
    public interface ITopicService
    {
        Task<TopicDto> CreateAsync(string name, string? description = null, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<GetTopicsResponce> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TopicDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task Update(int id, string name, string? description = null, CancellationToken cancellationToken = default);
    }
}