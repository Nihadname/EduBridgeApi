using AutoMapper;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZiggyCreatures.Caching.Fusion;

namespace LearningManagementSystem.Application.Implementations
{
    public class NoteService:INoteService
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

        public async Task<NoteReturnDto> Create(NoteCreateDto noteCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new CustomException(401, "Id", "User ID cannot be null");
            }
            var existedUser = await _userManager.Users
     .Include(u => u.Notes)
       .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser == null)
            {
                throw new CustomException(404, "User", "User  cannot be null or not  found");
            }
            if (existedUser.Notes.Any(s => s.Title.Equals(noteCreateDto.Title, StringComparison.OrdinalIgnoreCase)))
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
        public async Task<PaginationDto<NoteListItemDto>> GetAll(int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
           
            var notesQuery = await _unitOfWork.NoteRepository.GetQuery(s=>s.AppUserId==userId&&s.IsDeleted==false);
            if(!string.IsNullOrWhiteSpace(searchQuery))
            {
                notesQuery= notesQuery.Where(s => s.Title.Contains(searchQuery) || s.Description.Contains(searchQuery));
            }

            var totalCount = await notesQuery.CountAsync();

            var paginatedQuery =(IEnumerable<Note>)await notesQuery.ToListAsync();

            var mappedNotes = _mapper.Map<List<NoteListItemDto>>(paginatedQuery);

            var paginationResult = await PaginationDto<NoteListItemDto>.Create((IEnumerable<NoteListItemDto>)mappedNotes, pageNumber, pageSize, totalCount);

            return paginationResult;
        }
        public async Task<string> DeleteForUser(Guid Id)
        {
            var existedNote = await GetUserWithUserAndIdChecks(Id);
            await _unitOfWork.NoteRepository.Delete(existedNote);
            await _unitOfWork.Commit();
            return "succesfully deleted";
        }
        public async Task<NoteReturnDto> UpdateForUser(Guid id,NoteUpdateDto noteUpdateDto)
        {
          var existedNote=  await GetUserWithUserAndIdChecks(id);
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedUser = await _userManager.Users
                 .Include(u => u.Notes)
                 .FirstOrDefaultAsync(u => u.Id == userId);
            if(existedUser is null)
            {
                throw new CustomException(404, "User", "User  cannot be null or not found");
            }
            if (noteUpdateDto.Title is not null)
            {
                if (existedUser.Notes.Any(s => s.Title.Equals(noteUpdateDto.Title, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new CustomException(400, "Title", "User already has Title like this");
                }
            }
            _mapper.Map(noteUpdateDto, existedNote);
            await _unitOfWork.NoteRepository.Update(existedNote);
            await _unitOfWork.Commit(); 
            var MappedNote=_mapper.Map<NoteReturnDto>(existedNote);   
            return MappedNote;
        }
        private async Task<Note> GetUserWithUserAndIdChecks(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(440, "Invalid GUID provided.");
            }
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            var cacheKey = $"Note_{id}";
            var cachedNote = await _cache.GetOrSetAsync<Note>(cacheKey, async _ =>
            {
                var existedNote = await _unitOfWork.NoteRepository.GetEntity(s => s.IsDeleted == false && s.AppUserId == userId && s.Id == id);
                return existedNote;
            });
            if (cachedNote is null)
            {
                throw new CustomException(404, "Note", "Note not found");
            }
          return  cachedNote;
        }
        public async Task<NoteReturnDto> GetById(Guid id)
        {
            var existedNote = await GetUserWithUserAndIdChecks(id);
            var MappedNote = _mapper.Map<NoteReturnDto>(existedNote);
            return MappedNote;
        }
    }
}
