﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Note
{
    public record NoteUpdateDto
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string CategoryName { get; init; }
    }
}
