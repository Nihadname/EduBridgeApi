﻿using FluentValidation;
using LearningManagementSystem.Application.Dtos.Course;
using Microsoft.AspNetCore.Http;
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
            RuleFor(s=>s.difficultyLevel).NotNull()
                .IsInEnum().WithMessage("Payment status is invalid.");
            RuleFor(s => s.Duration).NotNull();
            RuleFor(s=>s.Language).NotNull()
                .IsInEnum().WithMessage("Language is invalid.");
            RuleFor(s => s.Requirements).NotEmpty();
            RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Salary must be a positive number.").NotEmpty();
            RuleFor(x => x.StartDate)
    .GreaterThanOrEqualTo(DateTime.UtcNow)
    .WithMessage("StartDate must be in the future.")
    .When(x => x.StartDate.HasValue);
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("StartDate must be in the future.")
                .When(x => x.StartDate.HasValue);

            RuleFor(x => x)
           .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate)
           .WithMessage("StartDate must be earlier than or equal to EndDate.");
            RuleFor(s => s.Duration).NotNull();
            RuleFor(s => s).Custom((c, context) =>
            {
                long maxSizeInBytes = 15 * 1024 * 1024;
                if (c.formFile == null || !c.formFile.ContentType.Contains("image/"))
                {
                    context.AddFailure("Image", "Only image files are accepted");
                    return;
                }

                if (c.formFile != null && c.formFile.Length > maxSizeInBytes)
                {
                    context.AddFailure("Image", "Data storage exceeds the maximum allowed size of 15 MB");
                }

            });
        }
       
    }
}
