using FluentValidation;
using LearningManagementSystem.Application.Dtos.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.CourseValidators
{
    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(s=>s.Name).MaximumLength(160).MinimumLength(2).NotEmpty();
            RuleFor(s=>s.Description).MaximumLength(250).MinimumLength(3).NotEmpty();
        }
    }
}
