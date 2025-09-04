using EgeTutor.API.DTOs;
using EgeTutor.Application.Interfaces;
using EgeTutor.Application.Services;
using EgeTutor.Core.Enums;
using EgeTutor.Core.Interfaces;
using EgeTutor.Core.Models;
using EgeTutor.Persistence.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EgeTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionService _questionService;

        public QuestionsController(ILogger<QuestionsController> logger,
                                    IQuestionService questionService)
        {
            _logger = logger;
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<ActionResult<GetQuestionsResponce>> GetAllQuestions(CancellationToken cancellationToken = default)
        {
            var questions = await _questionService.GetAllAsync(cancellationToken);
            if (questions == null)
            {
                return NotFound();
            }
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id, CancellationToken cancellationToken = default)
        {
            var question = await _questionService.GetByIdAsync(id, cancellationToken);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost]
        [AuthorizeRoles(Roles.Admin, Roles.Tutor)]
        public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var question = await _questionService.CreateAsync(createDto, cancellationToken);

            return CreatedAtAction(nameof(CreateQuestion), new { id = question.Id });
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(Roles.Admin, Roles.Tutor)]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _questionService.Update(id, updateDto);
            return NoContent(); // Статус 204

        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<IActionResult> DeleteQuestion(int id, CancellationToken cancellationToken = default)
        {
            await _questionService.DeleteAsync(id);
            return NoContent(); // 204 No Content
        }
    }
}
