using EgeTutor.Application.DTOs.TopicDTOs;
using EgeTutor.Application.Interfaces;
using EgeTutor.Application.Services;
using EgeTutor.Core.Enums;
using EgeTutor.Core.Models;
using EgeTutor.Persistence.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EgeTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicsController> _logger;

        public TopicsController(ITopicService topicService, ILogger<TopicsController> logger)
        {
            _topicService = topicService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GetTopicsResponce>> GetAllTopics(CancellationToken cancellationToken = default)
        {
            var topics = await _topicService.GetAllAsync(cancellationToken);
            return Ok(topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDto>> GetTopic(int id, CancellationToken cancellationToken = default)
        {
            var topicDto = await _topicService.GetByIdAsync(id, cancellationToken);
            if (topicDto == null)
            {
                return NotFound();
            }
            return Ok(topicDto);
        }

        [HttpPost]
        [AuthorizeRoles(Roles.Admin, Roles.Tutor)]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _topicService.CreateAsync(createDto.Name, createDto.Description, cancellationToken);
            return CreatedAtAction(nameof(GetTopic), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(Roles.Admin, Roles.Tutor)]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] UpdateTopicDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _topicService.Update(id, updateDto.Name, updateDto.Description, cancellationToken);
            return NoContent();

        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(Roles.Admin)]
        public async Task<IActionResult> DeleteTopic(int id, CancellationToken cancellationToken = default)
        {
            await _topicService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
