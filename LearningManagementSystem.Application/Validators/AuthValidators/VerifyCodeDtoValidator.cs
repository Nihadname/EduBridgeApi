using FluentValidation;
using LearningManagementSystem.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.AuthValidators
{
    public class VerifyCodeDtoValidator : AbstractValidator<VerifyCodeDto>
    {
        public VerifyCodeDtoValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(6);
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
