using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Parent
{
    public record ParentCreateDto
    {
        public string AppUserId { get; set; }
        public List<Guid> StudentIds { get; set; }
    }
}
