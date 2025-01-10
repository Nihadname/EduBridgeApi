using FluentValidation;
using LearningManagementSystem.Application.Dtos.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.FeeValidators
{
    public class FeeImageUploadDtoValidator : AbstractValidator<FeeImageUploadDto>
    {
        public FeeImageUploadDtoValidator()
        {
            RuleFor(s=>s.Id).NotEmpty();
            RuleFor(s => s).Custom((c, context) =>
            {
                long maxSizeInBytes = 15 * 1024 * 1024;
                if (c.image == null || !c.image.ContentType.Contains("image/"))
                {
                    context.AddFailure("Image", "Only image files are accepted");
                    return;
                }

                if (c.image != null && c.image.Length > maxSizeInBytes)
                {
                    context.AddFailure("Image", "Data storage exceeds the maximum allowed size of 15 MB");
                }

            });
        }
    }
}
