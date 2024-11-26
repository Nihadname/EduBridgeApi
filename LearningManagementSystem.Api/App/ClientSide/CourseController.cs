using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
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
        [HttpGet("GetAllAsSelectItem")]
        public async Task<IActionResult> GetAllAsSelectItem()
        {
            return Ok(await  _courseService.GetAllAsSelectItem());  
        }
    }
}
