using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return Ok(await _authService.Login(loginDto));
        }
        [HttpPut("UpdateImage")]
        public async Task<IActionResult> UpdateImage(UserUpdateImageDto userUpdateImageDto)
        {
            return Ok(await _authService.UpdateImage(userUpdateImageDto));
        }
    }
}
