using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Lesson
{
    public class LessonCreateDto
    {
        public string Title { get; set; }
        public DateTime ScheduledDate { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
