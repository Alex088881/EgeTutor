using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgeTutor.Application.DTOs.TopicDTOs
{
    public record CreateTopicDto(
    [Required][StringLength(100)] string Name,
    [StringLength(500)] string? Description
);
}
