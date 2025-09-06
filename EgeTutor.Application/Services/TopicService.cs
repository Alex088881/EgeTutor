using EgeTutor.Application.DTOs.TopicDTOs;
using EgeTutor.Application.Interfaces;
using EgeTutor.Core.Exceptions;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;

namespace EgeTutor.Application.Services
{
    public class TopicService(ITopicRepository topicRepository) : ITopicService
    {
        private readonly ITopicRepository _topicRepository = topicRepository;

        public async Task<GetTopicsResponce> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var topics = await _topicRepository.GetAllAsync(cancellationToken) ?? throw new Exception($"{nameof(GetAllAsync)}: topics is not found");
            var topicDtos =  topics.Select(t => new TopicDto(t.Id, t.Name, t.Description)).ToList();
            return new GetTopicsResponce(topicDtos);
        }

        public async Task<TopicDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _topicRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException(nameof(Topic), id);
            return (new TopicDto(result.Id, result.Name, result.Description));
        }

        public async Task<TopicDto> CreateAsync(string name, string? description = null, CancellationToken cancellationToken = default)
        {
            var newTopic = new Topic(name, description);
            await _topicRepository.AddAsync(newTopic, cancellationToken);
            return new TopicDto(newTopic.Id, newTopic.Name, newTopic.Description);
        }

        public async Task Update(int id, string name, string? description = null, CancellationToken cancellationToken = default)
        {
            var existingTopic = await _topicRepository.GetByIdAsync(id, cancellationToken) 
                ?? throw new NotFoundException(nameof(Topic), id);

            existingTopic.UpdateName(name);
            if (description is not null)
            {
                existingTopic.UpdateDescription(description);
            }

            await _topicRepository.Update(existingTopic, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _topicRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
