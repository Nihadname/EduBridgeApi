using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Entities
{
    public class Course:BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Lesson> lessons { get; set; }
    }
}
