using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Commands.Course.Create
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<CourseCreateOrUpdateReturnDto>>
    {
        private readonly ICourseService _courseService;

        public CreateCourseHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Result<CourseCreateOrUpdateReturnDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            return await _courseService.Create(request.CourseCreateDto);
        }
    }
}
