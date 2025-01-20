using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Commands.Course.Update
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Result<CourseCreateOrUpdateReturnDto>>
    {
        private readonly ICourseService _courseService;

        public UpdateCourseHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Result<CourseCreateOrUpdateReturnDto>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
          return await  _courseService.Update(request.id, request.CourseUpdateDto);
        }
    }
}
