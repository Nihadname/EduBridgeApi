﻿using LearningManagementSystem.Application.Features.Commands.Course.Create;
using LearningManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Course
{
    public record CourseCreateDto
    {
        public IFormFile formFile { get; init; }
        public  string  Name { get; init; }
        public string Description { get; init; }
        public DifficultyLevel difficultyLevel { get; set; }
        public TimeSpan Duration { get; set; }
        public Language Language { get; set; }
        public string Requirements { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

       
    }
    public enum Language
    {
        English,
        Azerbaijani
    }
}
