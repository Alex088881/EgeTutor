using EgeTutor.API.DTOs;
using EgeTutor.Application.Interfaces;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;

namespace EgeTutor.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;

        public QuestionService(IQuestionRepository questionRepository, ITopicRepository topicRepository)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
        }
        public async Task<GetQuestionsResponce> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var questions = await _questionRepository.GetAllAsync(cancellationToken) ?? throw new Exception("Questions not found");

            var questionsDto = questions
                .Select(q => new QuestionDto(
                    q.Id,
                    q.Text,
                    q.CorrectAnswer,
                    q.ImageUrl,
                    q.TopicId,
                    q.AnswerUrl))
                .ToList();
            return new GetQuestionsResponce(questionsDto);
        }


        public async Task<QuestionDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var question = await _questionRepository.GetByIdAsync(id, cancellationToken) ?? throw new Exception($"{nameof(GetByIdAsync)}: question with id{id} not found");

            var questionDto = new QuestionDto(question.Id, question.Text, question.CorrectAnswer, question.ImageUrl, question.TopicId, question.AnswerUrl);
            return questionDto;
        }

        public async Task<QuestionDto> CreateAsync(CreateQuestionDto createDto, CancellationToken cancellationToken = default)
        {
            var topicExists = await _topicRepository.ExistsAsync(createDto.TopicId, cancellationToken);
            if (!topicExists)
                throw new ArgumentException($"Topic with id {createDto.TopicId} does not exist");
            

            var question = new Question(
              createDto.Text,
              createDto.CorrectAnswer,
              createDto.TopicId,
              createDto.ImageUrl,
              createDto.AnswerUrl
              );
            await _questionRepository.AddAsync(question, cancellationToken);
            return new QuestionDto(
                question.Id,
                question.Text,
                question.CorrectAnswer,
                question.ImageUrl,
                question.TopicId,
                question.AnswerUrl
                );
        }

        public async Task Update(int id, UpdateQuestionDto updateDto, CancellationToken cancellationToken = default)
        {
            var existingQuestion = await _questionRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new Exception($"Question with id{id} not found");

            if (string.IsNullOrWhiteSpace(updateDto.Text))
                throw new ArgumentException("Text cannot be empty");

            if (string.IsNullOrWhiteSpace(updateDto.CorrectAnswer))
                throw new ArgumentException("CorrectAnswer cannot be empty");

            if (updateDto.TopicId <= 0)
                throw new ArgumentException("TopicId must be positive");

            existingQuestion.UpdateText(updateDto.Text);
            existingQuestion.UpdateCorrectAnswer(updateDto.CorrectAnswer);
            existingQuestion.UpdateImageUrl(updateDto.ImageUrl);
            existingQuestion.UpdateAnswerUrl(updateDto.AnswerUrl);
            existingQuestion.ChangeTopic(updateDto.TopicId);

            await _questionRepository.Update(existingQuestion, cancellationToken);
            return;

        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _questionRepository.DeleteAsync(id, cancellationToken);
            return;
        }
    }
}
