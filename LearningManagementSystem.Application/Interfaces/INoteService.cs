using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface INoteService
    {
        Task<Result<NoteReturnDto>> Create(NoteCreateDto noteCreateDto);
        Task<Result<PaginationDto<NoteListItemDto>>> GetAll(int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null);
        Task<Result<string>> DeleteForUser(Guid Id);
        Task<Result<NoteReturnDto>> UpdateForUser(Guid id, NoteUpdateDto noteUpdateDto);
        Task<Result<NoteReturnDto>> GetById(Guid id);
    }
}
