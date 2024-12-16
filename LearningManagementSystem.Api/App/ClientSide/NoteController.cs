using BenchmarkDotNet.Attributes;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Api.App.ClientSide
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Create(NoteCreateDto noteCreateDto)
        {
            return Ok(await _noteService.Create(noteCreateDto));
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Benchmark]
        public async Task<IActionResult> GetAll(int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null)
        {
            var result = await _noteService.GetAll(pageNumber, pageSize, searchQuery);
            return Ok(result);
        }
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteForUser(Guid Id)
        {
            return Ok(await  _noteService.DeleteForUser(Id)); 
        }
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Update(Guid id, [FromForm] NoteUpdateDto updateDto)
        {
            return Ok(await _noteService.UpdateForUser(id, updateDto));
        }
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Benchmark]
        public async Task<IActionResult> Get(Guid id)
        {
            
            return Ok(await _noteService.GetById(id));
        }
    }
}
