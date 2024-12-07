using FluentValidation;
using LearningManagementSystem.Application.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.ReportValidators
{
    public class ReportCreateDtoValidator : AbstractValidator<ReportCreateDto>
    {
        public ReportCreateDtoValidator()
        {
            RuleFor(s=>s.Description).NotEmpty().MinimumLength(4).MaximumLength(400);
            RuleFor(s => s.ReportOptionId).NotEmpty();
        }
    }
}
