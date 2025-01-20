using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Commands.Course.DeleteFromUi
{
    public record DeleteFromUiCourseCommand(Guid id) : IRequest<Result<string>>;

}
