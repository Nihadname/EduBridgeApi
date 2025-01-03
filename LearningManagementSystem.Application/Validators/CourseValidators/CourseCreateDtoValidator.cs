using FluentValidation;
using LearningManagementSystem.Application.Dtos.Course;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Validators.CourseValidators
{
    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.formFile)
               .NotNull().WithMessage("Bir resim dosyası zorunludur.")
               .Must(BeAValidImage).WithMessage("Sadece resim dosyalarına (jpg, jpeg, png) izin verilir.");
            RuleFor(s=>s.Name).MaximumLength(160).MinimumLength(2).NotEmpty();
            RuleFor(s=>s.Description).MaximumLength(250).MinimumLength(3).NotEmpty();
            RuleFor(s => s).Custom((c, context) =>
            {
                long maxSizeInBytes = 15 * 1024 * 1024;
                if (c.formFile == null || !c.formFile.ContentType.Contains("image/"))
                {
                    context.AddFailure("Image", "Only image files are accepted");
                    return;
                }

                if (c.formFile != null && c.formFile.Length > maxSizeInBytes)
                {
                    context.AddFailure("Image", "Data storage exceeds the maximum allowed size of 15 MB");
                }

            });
        }
        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
    }
}
