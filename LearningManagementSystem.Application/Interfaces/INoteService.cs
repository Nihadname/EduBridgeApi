using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Dtos.Paganation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface INoteService
    {
        Task<NoteReturnDto> Create(NoteCreateDto noteCreateDto);
        Task<PaginationDto<NoteListItemDto>> GetAll(int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null);
        Task<string> DeleteForUser(Guid Id);
        Task<NoteReturnDto> UpdateForUser(Guid id, NoteUpdateDto noteUpdateDto);
    }
}
