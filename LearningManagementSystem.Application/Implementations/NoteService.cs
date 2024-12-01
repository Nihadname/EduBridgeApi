using AutoMapper;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class NoteService:INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<NoteReturnDto> Create(NoteCreateDto noteCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            var existedUser = await _userManager.Users
     .Include(u => u.Notes)
     .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser == null)
            {
                throw new CustomException(400, "User", "User  cannot be null");
            }
            if (existedUser.Notes.Select(s => s.Title).Equals(noteCreateDto.Title))
            {
                throw new CustomException(400, "Title", "User already has Title like this");
            }
            noteCreateDto.AppUserId = userId;
            var MappedNote = _mapper.Map<Note>(noteCreateDto);
            await _unitOfWork.NoteRepository.Create(MappedNote);
            await _unitOfWork.Commit();
            var MappedResponse=_mapper.Map<NoteReturnDto>(MappedNote);
            return MappedResponse;
        }
    }
}
