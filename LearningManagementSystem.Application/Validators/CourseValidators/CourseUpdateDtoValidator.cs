using FluentValidation;
using LearningManagementSystem.Application.Dtos.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.CourseValidators
{
    public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(160)
            .When(x => x.Name != null);

            RuleFor(x => x.Description)
         .NotEmpty()
         .MinimumLength(4)
         .MaximumLength(250)
         .When(x => x.Description != null);

        }
    }
}
