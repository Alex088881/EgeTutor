using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using EgeTutor.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EgeTutor.Persistence.Repisitories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            await _context.Topics.AddAsync(topic, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Topic>?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Topics.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Topic?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Topics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Update(Topic topic, CancellationToken cancellationToken = default)
        {
            var existingTopic = await _context.Topics.FindAsync(topic.Id, cancellationToken) 
                ?? throw new Exception($"Topic with id {topic.Id} not found");

            existingTopic.UpdateName(topic.Name);
            if (topic.Description is not null)
            {
                existingTopic.UpdateDescription(topic.Description);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var topic = await _context.Topics.FindAsync(id, cancellationToken);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Topics
                .AnyAsync(t => t.Id == id, cancellationToken);
        }
    }
}
