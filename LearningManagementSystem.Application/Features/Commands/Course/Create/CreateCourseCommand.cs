using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;

namespace LearningManagementSystem.Application.Features.Commands.Course.Create
{
    public record CreateCourseCommand(CourseCreateDto CourseCreateDto) : IRequest<Result<CourseCreateOrUpdateReturnDto>>;

}
