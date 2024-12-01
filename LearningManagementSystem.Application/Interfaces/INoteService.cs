using LearningManagementSystem.Application.Dtos.Note;
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
    }
}
