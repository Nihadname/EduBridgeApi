using LearningManagementSystem.Application.Dtos.ReportOption;
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
    public class ReportOptionController : ControllerBase
    {
        private readonly IReportOptionService _optionService;

        public ReportOptionController(IReportOptionService optionService)
        {
            _optionService = optionService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

        public async Task<IActionResult> Create([FromForm]ReportOptionCreateDto reportOptionCreateDto)
        {
            return Ok(await _optionService.Create(reportOptionCreateDto));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _optionService.Delete(id));
        }
        [HttpDelete("Ui/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteFromUi(Guid id)
        {
            return Ok(await _optionService.DeleteFromUi(id));
        }
    }
}
