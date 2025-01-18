using AutoMapper;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZiggyCreatures.Caching.Fusion;

namespace LearningManagementSystem.Application.Implementations
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFusionCache _cache;
        public NoteService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IFusionCache cache)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<Result<NoteReturnDto>> Create(NoteCreateDto noteCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<NoteReturnDto>.Failure("UserId", "User ID cannot be null", ErrorType.ValidationError);
            }
            var existedUser = await _userManager.Users
     .Include(u => u.Notes)
       .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser == null)
            {
                return Result<NoteReturnDto>.Failure("User", "User  cannot be null or not  found", ErrorType.NotFoundError);
            }
            if (existedUser.Notes.Any(s => s.Title.Equals(noteCreateDto.Title, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<NoteReturnDto>.Failure("Title", "User already has Title like this", ErrorType.BusinessLogicError);
            }
            noteCreateDto.AppUserId = userId;
            var MappedNote = _mapper.Map<Note>(noteCreateDto);
            await _unitOfWork.NoteRepository.Create(MappedNote);
            await _unitOfWork.Commit();
            var MappedResponse = _mapper.Map<NoteReturnDto>(MappedNote);
            return Result<NoteReturnDto>.Success(MappedResponse);
        }
        public async Task<Result<PaginationDto<NoteListItemDto>>> GetAll(int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Result<PaginationDto<NoteListItemDto>>.Failure("UserId", "User ID cannot be null", ErrorType.ValidationError);
            }

            var notesQuery = await _unitOfWork.NoteRepository.GetQuery(s => s.AppUserId == userId && s.IsDeleted == false);
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                notesQuery = notesQuery.Where(s => s.Title.Contains(searchQuery) || s.Description.Contains(searchQuery));
            }

             notesQuery=notesQuery.OrderByDescending(s => s.CreatedTime);
            var paginationResult = await PaginationDto<NoteListItemDto>
                .Create(
                notesQuery.Select(f => _mapper.Map<NoteListItemDto>(f)), pageNumber, pageSize);

            return Result<PaginationDto<NoteListItemDto>>.Success(paginationResult);
        }
        public async Task<Result<string>> DeleteForUser(Guid Id)
        {
            var existedNoteResult = await GetUserWithUserAndIdChecks(Id);
            if (!existedNoteResult.IsSuccess)
            {
                return Result<string>.Failure(existedNoteResult.ErrorKey, existedNoteResult.Message, (ErrorType)existedNoteResult.ErrorType);
            }
            var existedNote = existedNoteResult.Data;
            await _unitOfWork.NoteRepository.Delete(existedNote);
            await _unitOfWork.Commit();
            return Result<string>.Success("deleted by user");
        }
        public async Task<Result<NoteReturnDto>> UpdateForUser(Guid id, NoteUpdateDto noteUpdateDto)
        {
            var existedNoteResult = await GetUserWithUserAndIdChecks(id);
            if (!existedNoteResult.IsSuccess)
            {
                return Result<NoteReturnDto>.Failure(existedNoteResult.ErrorKey, existedNoteResult.Message, (ErrorType)existedNoteResult.ErrorType);
            }
            var existedNote = existedNoteResult.Data;
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedUser = await _userManager.Users
                 .Include(u => u.Notes)
                 .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser is null)
            {
                return Result<NoteReturnDto>.Failure("User", "User  cannot be null or not  found", ErrorType.NotFoundError);
            }
            if (noteUpdateDto.Title is not null)
            {
                if (existedUser.Notes.Any(s => s.Title.Equals(noteUpdateDto.Title, StringComparison.OrdinalIgnoreCase)))
                {
                    return Result<NoteReturnDto>.Failure("Title", "User already has Title like this", ErrorType.BusinessLogicError);

                }
            }
            _mapper.Map(noteUpdateDto, existedNote);
            await _unitOfWork.NoteRepository.Update(existedNote);
            await _unitOfWork.Commit();
            var MappedNote = _mapper.Map<NoteReturnDto>(existedNote);
            return Result<NoteReturnDto>.Success(MappedNote);
        }
        private async Task<Result<Note>> GetUserWithUserAndIdChecks(Guid id)
        {
            if (id == Guid.Empty)
            {
               return Result<Note>.Failure(null, "Invalid GUID provided.", ErrorType.ValidationError);
            }
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
               return Result<Note>.Failure("Id", "User ID cannot be null", ErrorType.ValidationError);
            }
            var cacheKey = $"Note_{id}";
            var cachedNote = await _cache.GetOrSetAsync<Note>(cacheKey, async _ =>
            {
                var existedNote = await _unitOfWork.NoteRepository.GetEntity(s => s.IsDeleted == false && s.AppUserId == userId && s.Id == id);
                return existedNote;
            });
            if (cachedNote is null)
            {
             return   Result<Note>.Failure("Note", "Note not found", ErrorType.NotFoundError);
            }
            return Result<Note>.Success(cachedNote);
        }
        public async Task<Result<NoteReturnDto>> GetById(Guid id)
        {
            var existedNoteResult = await GetUserWithUserAndIdChecks(id);
            if (!existedNoteResult.IsSuccess)
            {
                return Result<NoteReturnDto>.Failure(existedNoteResult.ErrorKey, existedNoteResult.Message, (ErrorType)existedNoteResult.ErrorType);
            }
            var existedNote = existedNoteResult.Data;
            var MappedNote = _mapper.Map<NoteReturnDto>(existedNote);
            return Result<NoteReturnDto>.Success(MappedNote);
        }
    }
}
