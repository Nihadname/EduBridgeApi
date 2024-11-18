﻿using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Entities
{
    public class Note:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
    }
}
