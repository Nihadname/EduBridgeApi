using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CourseCreateDto courseCreateDto)
        {
            return Ok(await _courseService.Create(courseCreateDto));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CourseUpdateDto courseUpdateDto)
        {
            return Ok(await _courseService.Update(id, courseUpdateDto));
        }
    }
}
