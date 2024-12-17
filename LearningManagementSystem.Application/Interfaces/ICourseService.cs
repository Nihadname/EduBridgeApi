using LearningManagementSystem.Application.Dtos.Course;
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
        Task<CourseReturnDto> Create(CourseCreateDto courseCreateDto);
        Task<CourseReturnDto> Update(Guid id, CourseUpdateDto courseUpdateDto);
        Task<CourseReturnDto> GetById(Guid id);
        Task<string> DeleteFromUi(Guid id);
        Task<string> Delete(Guid id);
        Task<PaginationDto<CourseListItemDto>> GetAll(int pageNumber = 1,
           int pageSize = 10,
           string searchQuery = null);
    }
}
