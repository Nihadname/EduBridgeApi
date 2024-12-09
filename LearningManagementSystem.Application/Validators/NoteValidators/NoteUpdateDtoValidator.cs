using FluentValidation;
using LearningManagementSystem.Application.Dtos.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.NoteValidators
{
    public class NoteUpdateDtoValidator : AbstractValidator<NoteUpdateDto>
    {
        public NoteUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .When(x => x.Title != null);
            RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(160)
            .When(x => x.CategoryName != null);
            RuleFor(x => x.Description)
         .NotEmpty()
         .MinimumLength(4)
         .MaximumLength(250)
         .When(x => x.Description != null);
        }
    }
}
