using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using EgeTutor.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EgeTutor.Persistence.Repisitories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Question question, CancellationToken cancellationToken = default)
        {
            if (question is null)
                throw new ArgumentNullException(nameof(question));
            await _context.Questions.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions.FindAsync(id, cancellationToken);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Question>?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Questions.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<Question>?> GetAllQuestionsOfTopicAsinc(int topicId, CancellationToken cancellationToken = default)
        {
            return await _context.Questions
             .AsNoTracking()
             .Where(q => q.TopicId == topicId)
            .ToListAsync(cancellationToken);
        }

        public async Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Questions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Update(Question question, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(question);

            var existingQuestion = await _context.Questions.FindAsync(question.Id, cancellationToken)
                ?? throw new Exception($"Question with id {question.Id} not found");

            existingQuestion.UpdateText(question.Text);
            existingQuestion.UpdateCorrectAnswer(question.CorrectAnswer);
            existingQuestion.UpdateImageUrl(question.ImageUrl);
            existingQuestion.UpdateAnswerUrl(question.AnswerUrl);
            existingQuestion.ChangeTopic(question.TopicId);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
