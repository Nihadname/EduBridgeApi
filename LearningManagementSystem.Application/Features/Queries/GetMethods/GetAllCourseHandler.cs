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
    public class GetAllCourseHandler : IRequestHandler<GetAllCourseQuery, Result<PaginationDto<CourseListItemDto>>>
    {
        private readonly ICourseService _courseService;

        public GetAllCourseHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Result<PaginationDto<CourseListItemDto>>> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
        {
            return await _courseService.GetAll(request.TeacherIds, request.pageNumber, request.pageSize, request.searchQuery);
        }
    }
}
