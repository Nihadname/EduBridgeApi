using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Features.Commands.Course.Create;
using LearningManagementSystem.Application.Features.Commands.Course.Delete;
using LearningManagementSystem.Application.Features.Commands.Course.DeleteFromUi;
using LearningManagementSystem.Application.Features.Commands.Course.Update;
using LearningManagementSystem.Application.Features.Queries.GetMethods;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> Get(GetCourseByIdQuery getCourseByIdQuery)
        {
            var result=await _mediator.Send(getCourseByIdQuery);
            return Ok(result);
        }
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
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll(GetAllCourseQuery getAllCourseQuery)
        {
            var result = await _mediator.Send(getAllCourseQuery);
            return Ok(result);
        }
    }
}
