using LearningManagementSystem.Application.Features.Queries.GetMethods;
using LearningManagementSystem.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMediator _mediator;
        public CourseController(ICourseService courseService, IMediator mediator)
        {
            _courseService = courseService;
            _mediator = mediator;
        }
        [HttpGet("GetAllAsSelectItem")]
        public async Task<IActionResult> GetAllAsSelectItem()
        {
            return Ok(await  _courseService.GetAllAsSelectItem());  
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(GetAllCourseQuery getAllCourseQuery)
        {
            var result = await _mediator.Send(getAllCourseQuery);
            return Ok(result);
        }
    }
}
