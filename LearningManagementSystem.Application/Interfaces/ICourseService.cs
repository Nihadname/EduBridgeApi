using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseSelectItemDto>> GetAllAsSelectItem();
        Task<Result<CourseCreateOrUpdateReturnDto>> Create(CourseCreateDto courseCreateDto);
        Task<Result<CourseCreateOrUpdateReturnDto>> Update(Guid id, CourseUpdateDto courseUpdateDto);
        Task<Result<CourseReturnDto>> GetById(Guid id);
        Task<Result<string>> DeleteFromUi(Guid id);
        Task<Result<string>> Delete(Guid id);
     Task<Result<PaginationDto<CourseListItemDto>>> GetAll(List<Guid> TeacherIds, int pageNumber = 1,
          int pageSize = 10,
          string searchQuery = null);
    }
}
