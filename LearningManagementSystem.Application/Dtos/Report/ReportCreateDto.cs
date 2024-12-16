using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Report
{
    public class ReportCreateDto
    {
        public string Description { get; set; }
        [JsonIgnore]
        public string AppUserId { get; set; }
        public string ReportedUserId { get; set; }
        public Guid ReportOptionId { get; set; }
    }
}
