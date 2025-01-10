using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
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
        [HttpPut("UploadImageOfBankTransfer")]
        public async Task<IActionResult> UploadImageOfBankTransfer(FeeImageUploadDto feeImageUploadDto)
        {
            return Ok(await _feeService.UploadImageOfBankTransfer(feeImageUploadDto));
        }
    }
}
