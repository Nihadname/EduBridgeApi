using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Features.Commands.Course.DeleteFromUi
{
    public class DeleteFromUiCourseHandler : IRequestHandler<DeleteFromUiCourseCommand, Result<string>>
    {
        private readonly ICourseService _courseService;

        public DeleteFromUiCourseHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<Result<string>> Handle(DeleteFromUiCourseCommand request, CancellationToken cancellationToken)
        {
            return await _courseService.DeleteFromUi(request.id);
        }
    }
}
