using FluentValidation;
using LearningManagementSystem.Application.Dtos.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.NoteValidators
{
    public class NoteCreateDtoValidator : AbstractValidator<NoteCreateDto>
    {
        public NoteCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(10).MaximumLength(90).WithMessage("Title must be at least 10 characters long.");
            RuleFor(x => x.Description).NotEmpty().MinimumLength(10).MaximumLength(180).WithMessage("Description must be at least 10 characters long.");
            RuleFor(x => x.CategoryName)
              .NotEmpty().WithMessage("CategoryName is required.");
        }
    }
}
