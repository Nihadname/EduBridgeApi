using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadImageOfBankTransfer(FeeImageUploadDto feeImageUploadDto)
        {
            return Ok(await _feeService.UploadImageOfBankTransfer(feeImageUploadDto));
        }
        [HttpPost("ProcessPayment/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ProcessPayment(Guid id,FeeHandleDto feeHandleDto)
        {
            return Ok(await _feeService.ProcessPayment(id, feeHandleDto));
        }
        [HttpGet("GetAllFeesForUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllFeesForUser(DateTime? startPaidDate, DateTime? endPaidDateTime, int pageNumber = 1,
           int pageSize = 10)
        {
            return Ok(await _feeService.GetAllOfUsersFees(startPaidDate, endPaidDateTime, pageNumber, pageSize));
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _feeService.GetById(id));
        }
    }
}
