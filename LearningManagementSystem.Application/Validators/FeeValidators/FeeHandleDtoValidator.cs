using FluentValidation;
using LearningManagementSystem.Application.Dtos.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.FeeValidators
{
    public class FeeHandleDtoValidator : AbstractValidator<FeeHandleDto>
    {
        public FeeHandleDtoValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(160)
            .When(x => x.Description != null);
            RuleFor(s => s.PaymentMethodId).NotEmpty();
        }
    }
}
