using LearningManagementSystem.Application.Dtos.RequstToRegister;
using LearningManagementSystem.Application.Interfaces;
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
           
                return Ok(await _requstToRegisterService.Create(requstToRegisterCreateDto));
           
        }
    }
}
