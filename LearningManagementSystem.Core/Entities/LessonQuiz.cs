﻿using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Entities
{
    public class LessonQuiz:BaseEntity
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public Guid? LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
