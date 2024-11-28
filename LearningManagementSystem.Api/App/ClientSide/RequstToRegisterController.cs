using LearningManagementSystem.Application.Dtos.RequstToRegister;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequstToRegisterController : ControllerBase
    {
        private readonly IRequstToRegisterService _requstToRegisterService;

        public RequstToRegisterController(IRequstToRegisterService requstToRegisterService)
        {
            _requstToRegisterService = requstToRegisterService;
        }

        [HttpPost]
        public  async Task<IActionResult> Create([FromForm]RequstToRegisterCreateDto requstToRegisterCreateDto)
        {
            return Ok(new { message = await _requstToRegisterService.Create(requstToRegisterCreateDto) });
        }
        [HttpGet("VerifyExistenceOfEmailUser")]
        public async Task<IActionResult> VerifyExistenceOfEmailUser([FromQuery]string token)
        {
            return Ok(new { message = await _requstToRegisterService.VerifyExistenceOfEmailUser(token) });
        }

    }
}
