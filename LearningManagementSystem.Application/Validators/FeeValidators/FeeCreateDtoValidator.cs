using FluentValidation;
using LearningManagementSystem.Application.Dtos.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.FeeValidators
{
    public class FeeCreateDtoValidator : AbstractValidator<FeeCreateDto>
    {
        public FeeCreateDtoValidator()
        {
            RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Salary must be a positive number.").NotEmpty();
            RuleFor(s => s.DueDate).NotNull().GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(s=>s.PaymentStatus).NotNull()
                .IsInEnum().WithMessage("Payment status is invalid.");
            RuleFor(s=>s.StudentId).NotNull().NotEmpty();
        }
    }
}
