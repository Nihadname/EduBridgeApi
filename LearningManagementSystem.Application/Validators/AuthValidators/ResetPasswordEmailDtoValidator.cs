using FluentValidation;
using LearningManagementSystem.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.AuthValidators
{
    public class ResetPasswordEmailDtoValidator : AbstractValidator<ResetPasswordEmailDto>
    {
        public ResetPasswordEmailDtoValidator()
        {
            RuleFor(s => s.Email).NotEmpty().WithMessage("not empty")
             .MinimumLength(8);
        }
    }
}
