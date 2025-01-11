using CloudinaryDotNet.Actions;
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
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoOrVideoService _photoOrVideoService;

        public PhotoController(IPhotoOrVideoService photoOrVideoService)
        {
            _photoOrVideoService = photoOrVideoService;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile formFile)
        {
            return Ok(await _photoOrVideoService.UploadMediaAsync(formFile, true));
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string mediaUrl)
        {
            return Ok(await _photoOrVideoService.DeleteMediaAsync(mediaUrl, ResourceType.Video));
        }
    }
}
