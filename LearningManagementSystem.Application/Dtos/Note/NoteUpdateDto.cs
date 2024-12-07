using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Note
{
    public record NoteUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
    }
}
