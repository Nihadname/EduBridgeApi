using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Features.Commands.Course.Create;
using LearningManagementSystem.Application.Features.Commands.Course.Delete;
using LearningManagementSystem.Application.Features.Commands.Course.DeleteFromUi;
using LearningManagementSystem.Application.Features.Commands.Course.Update;
using LearningManagementSystem.Application.Interfaces;
using MediatR;
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
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCourseCommand  createCourseCommand)
        {
            var result = await _mediator.Send(createCourseCommand);
            return Ok(result);

        }
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> Update([FromForm] UpdateCourseCommand updateCourseCommand)
        {
            var result=await _mediator.Send(updateCourseCommand);
            return Ok(result);
        }
        //[HttpGet("{id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        //public async Task<IActionResult> Get(Guid id)
        //{
        //    return Ok(await _courseService.GetById(id));
        //}
        [HttpDelete("Ui/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteFromUi(DeleteFromUiCourseCommand deleteFromUiCourseCommand)
        {
            var result = await _mediator.Send(deleteFromUiCourseCommand);
            return Ok(result);
        }
        [HttpDelete("{id}")]
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult>  Delete(DeleteCourseCommand deleteCourseCommand)
        {
            var result=await _mediator.Send(deleteCourseCommand);
           return Ok(result);
        }
        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAll([FromQuery] List<Guid> TeacherIds, int pageNumber = 1,
        //   int pageSize = 10,
        //   string searchQuery = null)
        //{
        //    return Ok(await _courseService.GetAll(TeacherIds,pageNumber, pageSize, searchQuery));  
        //}
    }
}
