using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Commands.Course.Delete
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result<string>>
    {
        private readonly ICourseService _courseService;

        public DeleteCourseHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Result<string>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            return await _courseService.Delete(request.id);
        }
    }
}
