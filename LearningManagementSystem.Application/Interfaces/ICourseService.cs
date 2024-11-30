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
    }
}
