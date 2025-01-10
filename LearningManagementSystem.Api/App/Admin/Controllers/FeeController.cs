using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeController : ControllerBase
    {
        private readonly IFeeService _feeService;

        public FeeController(IFeeService feeService)
        {
            _feeService = feeService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]FeeCreateDto feeCreateDto)
        {
            return Ok(await _feeService.CreateFeeAndAssignToStudent(feeCreateDto));
        }
    }
}
