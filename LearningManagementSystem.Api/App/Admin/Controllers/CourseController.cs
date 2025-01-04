using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> Update(Guid id, [FromForm] CourseUpdateDto courseUpdateDto)
        {
            return Ok(await _courseService.Update(id, courseUpdateDto));
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _courseService.GetById(id));
        }
        [HttpDelete("Ui/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteFromUi(Guid id)
        {
            return Ok(await _courseService.DeleteFromUi(id));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult>  Delete(Guid id)
        {
            return Ok(await _courseService.Delete(id));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(List<Guid> TeacherIds, int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null)
        {
            return Ok(await _courseService.GetAll(TeacherIds,pageNumber, pageSize, searchQuery));  
        }
    }
}
