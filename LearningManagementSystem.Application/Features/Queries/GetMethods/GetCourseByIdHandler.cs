using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Queries.GetMethods
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseReturnDto>>
    {
        private readonly ICourseService _courseService;

        public GetCourseByIdHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Result<CourseReturnDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _courseService.GetById(request.Id);
        }
    }
}
