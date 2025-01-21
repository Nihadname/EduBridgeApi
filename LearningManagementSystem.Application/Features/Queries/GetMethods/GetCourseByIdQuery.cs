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
    public record GetCourseByIdQuery(Guid Id) : IRequest<Result<CourseReturnDto>>;
}
