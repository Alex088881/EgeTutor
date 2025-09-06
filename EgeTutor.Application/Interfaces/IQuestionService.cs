

using EgeTutor.Application.DTOs;

namespace EgeTutor.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionDto> CreateAsync(CreateQuestionDto createDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<GetQuestionsResponce> GetAllAsync(CancellationToken cancellationToken = default);
        Task<QuestionDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task Update(int id, UpdateQuestionDto updateDto, CancellationToken cancellationToken = default);
    }
}