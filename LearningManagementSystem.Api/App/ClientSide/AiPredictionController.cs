using LearningManagementSystem.Application.Dtos.Ai;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiPredictionController : ControllerBase
    {
        private readonly IAiPredictionService _aiPredictionService;

        public AiPredictionController(IAiPredictionService aiPredictionService)
        {
            _aiPredictionService = aiPredictionService;
        }
        [HttpPost("predict-course")]
        public async Task<IActionResult> PredictCourse([FromBody]UserData userData)
        {
            return Ok(await _aiPredictionService.PredictCourse(userData));

        }
    }
}
