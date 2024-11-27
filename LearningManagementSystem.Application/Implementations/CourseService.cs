using AutoMapper;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class CourseService:ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CourseReturnDto> Create(CourseCreateDto courseCreateDto)
        {
            if(await _unitOfWork.CourseRepository.isExists(s=>s.Name.ToLower() == courseCreateDto.Name.ToLower()))
            {
                throw new CustomException(400, "Name", "There is an already existed course");
            }
           var MappedCourse=_mapper.Map<Course>(courseCreateDto);
            await _unitOfWork.CourseRepository.Create(MappedCourse);
            await _unitOfWork.Commit();
            var ResponseCourseDto=_mapper.Map<CourseReturnDto>(MappedCourse);
            return ResponseCourseDto;
        }
        public async Task<CourseReturnDto> Update(Guid id,CourseUpdateDto courseUpdateDto)
        {
            if (courseUpdateDto.Name != null)
            {
                if (await _unitOfWork.CourseRepository.isExists(s => s.Name.ToLower() == courseUpdateDto.Name.ToLower()))
                {
                    throw new CustomException(400, "Name", "There is an already existed course");
                }
            }
            var ExistedCourse = await _unitOfWork.CourseRepository.GetEntity(s =>s.Id == id);
            if(ExistedCourse == null)
            {
                throw new CustomException(400, "Course doesnt exist");
            }
            _mapper.Map(courseUpdateDto, ExistedCourse);
            await _unitOfWork.CourseRepository.Update(ExistedCourse);
            await _unitOfWork.Commit();
            var ResponseCourseDto = _mapper.Map<CourseReturnDto>(ExistedCourse);
            return ResponseCourseDto;
        }
        public async Task<List<CourseSelectItemDto>> GetAllAsSelectItem()
        {
            var allCourses= await _unitOfWork.CourseRepository.GetAll(s=>s.IsDeleted == false);
            var MappedCourses=  _mapper.Map<List<CourseSelectItemDto>>(allCourses);
            return MappedCourses;
        }
    }
}
