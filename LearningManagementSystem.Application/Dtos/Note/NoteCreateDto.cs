using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Note
{
    public record NoteCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        [JsonIgnore]
        public string AppUserId { get; set; }
    }
}
