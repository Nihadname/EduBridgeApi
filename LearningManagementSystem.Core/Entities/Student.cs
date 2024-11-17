using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Entities
{
    public class Student:BaseEntity
    { 
        public decimal? AvarageScore { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid? ParentId { get; set; }
        public Parent Parent { get; set; }
        public ICollection<LessonStudent> lessonStudents { get; set; }

    }
}
