using FluentValidation;
using LearningManagementSystem.Application.Dtos.ReportOption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.ReportOptionValidators
{
    public class ReportOptionCreateDtoValidator:AbstractValidator<ReportOptionCreateDto>
    {
        public ReportOptionCreateDtoValidator()
        {
            RuleFor(s=>s.Name).NotEmpty().MaximumLength(300);  
        }
    }
}
