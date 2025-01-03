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
    }
}
