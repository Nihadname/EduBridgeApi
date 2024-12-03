using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            return Ok(await _authService.ChangePassword(changePasswordDto));
        }
        [HttpPost("ResetPasswordSendEmail")]
        public async Task<IActionResult> ResetPasswordSendEmail(ResetPasswordEmailDto resetPasswordEmailDto)
        {
          
            var result = await _authService.ResetPasswordSendEmail(resetPasswordEmailDto);
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email, string token, ResetPasswordDto resetPasswordDto)
        {
            return Ok(await _authService.ResetPassword(email, token, resetPasswordDto));    
        }
        [HttpGet("CheckAuth")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> CheckAuth()
        {
            return Ok(new {userName= await _authService.GetUserName() });
        }
        [HttpGet("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Profile()
        {
            return Ok(await _authService.Profile());
        }
    }
}
