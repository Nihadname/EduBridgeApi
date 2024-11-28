﻿using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.Admin.Controllers
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
        [HttpGet("SendAcceptanceEmail/{id}")]
        public async Task<IActionResult> SendAcceptanceEmail(Guid id)
        {
            return Ok(await _requstToRegisterService.SendAcceptanceEmail(id));
        }
    }
}
