using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Queries.GetMethods
{
    public record GetAllCourseQuery(List<Guid> TeacherIds, int pageNumber = 1,
          int pageSize = 10,
           string searchQuery = null) : IRequest<Result<PaginationDto<CourseListItemDto>>>;
}
