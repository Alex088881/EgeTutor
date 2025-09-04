using EgeTutor.Core.Models;

namespace EgeTutor.Application.DTOs.TopicDTOs
{
    public record TopicDto(
            int Id,
            string Name,
            string? Description
        );
}
