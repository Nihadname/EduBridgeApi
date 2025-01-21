using FluentValidation;
using LearningManagementSystem.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.AuthValidators
{
    internal class ResetPasswordTokenAndEmailDtoValidator : AbstractValidator<ResetPasswordTokenAndEmailDto>
    {
        public ResetPasswordTokenAndEmailDtoValidator()
        {

            RuleFor(s => s.Email).NotEmpty().EmailAddress();
            RuleFor(s => s.Token).NotEmpty();
        }
    }
}
