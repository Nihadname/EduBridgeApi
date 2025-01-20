using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Commands.Course.Update
{
    public record UpdateCourseCommand(Guid id,CourseUpdateDto CourseUpdateDto) : IRequest<Result<CourseCreateOrUpdateReturnDto>>;

}
