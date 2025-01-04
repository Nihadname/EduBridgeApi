using AutoMapper;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Application.Implementations
{
    public class CourseService:ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoOrVideoService _photoOrVideoService;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoOrVideoService photoOrVideoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoOrVideoService = photoOrVideoService;
        }
        public async Task<CourseCreateReturnDto> Create(CourseCreateDto courseCreateDto)
        {
            if(await _unitOfWork.CourseRepository.isExists(s=>s.Name.ToLower() == courseCreateDto.Name.ToLower()))
            {
                throw new CustomException(400, "Name", "There is an already existed course");
            }
            
           var MappedCourse=_mapper.Map<Course>(courseCreateDto);
            MappedCourse.ImageUrl = await _photoOrVideoService.UploadMediaAsync(courseCreateDto.formFile, false);
            await _unitOfWork.CourseRepository.Create(MappedCourse);
            await _unitOfWork.Commit();
            var ResponseCourseDto=_mapper.Map<CourseCreateReturnDto>(MappedCourse);
            return ResponseCourseDto;
        }
        public async  Task<CourseReturnDto> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(440, "Invalid GUID provided.");
            }
            var ExistedCourse=await _unitOfWork.CourseRepository.GetEntity(s=>s.Id==id&& s.IsDeleted == false, includes: new Func<IQueryable<Course>, IQueryable<Course>>[] {
                 query => query
            .Include(p => p.lessons)
            });
            if (ExistedCourse is null)
            {
                throw new CustomException(404, "Course", "Not found");
            }
            var MappedCourse = _mapper.Map<CourseReturnDto>(ExistedCourse);
            return MappedCourse;
        }
        public async Task<CourseCreateReturnDto> Update(Guid id,CourseUpdateDto courseUpdateDto)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(440, "Invalid GUID provided.");
            }
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
            var ResponseCourseDto = _mapper.Map<CourseCreateReturnDto>(ExistedCourse);
            return ResponseCourseDto;
        }
        public async Task<List<CourseSelectItemDto>> GetAllAsSelectItem()
        {
            var allCourses= await _unitOfWork.CourseRepository.GetAll(s=>s.IsDeleted == false);
            var MappedCourses=  _mapper.Map<List<CourseSelectItemDto>>(allCourses);
            return MappedCourses;
        }
        public async Task<string> DeleteFromUi(Guid id)
        {
          var existedCourse= await GetCourseById(id);
            if (existedCourse.IsDeleted is true) throw new CustomException(400, "Course", "this already existed");
            existedCourse.IsDeleted = true;
            await _unitOfWork.Commit();
            return "Deleted";
        }
        public async Task<string> Delete(Guid id)
        {
            var existedCourse = await GetCourseById(id);
      await _unitOfWork.CourseRepository.Delete(existedCourse);
            await _unitOfWork.Commit();
            return "Deleted";
        }
        public async Task<PaginationDto<CourseListItemDto>> GetAll(List<Guid> TeacherIds, int pageNumber = 1,
           int pageSize = 10, 
           string searchQuery = null)
        {
            var courseQuery = await _unitOfWork.CourseRepository.GetQuery(s => s.IsDeleted == false, includes: new Func<IQueryable<Course>, IQueryable<Course>>[] {
                 query => query
            .Include(p => p.lessons)
            });
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                courseQuery = courseQuery.Where(s => s.Name.Contains(searchQuery) || s.Description.Contains(searchQuery));
            }
            if(TeacherIds.Any())
            {
                foreach (var TeacherId in TeacherIds)
                {
                    if(!await _unitOfWork.TeacherRepository.isExists(s=>s.Id== TeacherId)) throw new CustomException(400, "TeacherId", "TeacherId is invalid");
                    courseQuery = courseQuery.Where(s => s.lessons.Any(s => s.TeacherId == TeacherId));
                }
            }
            var totalCount = await courseQuery.CountAsync();

            var paginatedQuery = (IEnumerable<Course>)await courseQuery.ToListAsync();

            var mappedNotes = _mapper.Map<List<CourseListItemDto>>(paginatedQuery);

            var paginationResult = await PaginationDto<CourseListItemDto>.Create((IEnumerable<CourseListItemDto>)mappedNotes, pageNumber, pageSize, totalCount);

            return paginationResult;
        }
        private async Task<Course> GetCourseById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(440, "Invalid GUID provided.");
            }
            var ExistedCourse = await _unitOfWork.CourseRepository.GetEntity(s => s.Id == id && s.IsDeleted == false);
            if (ExistedCourse is null)
            {
                throw new CustomException(404, "Course", "Not found");
            }
            return ExistedCourse;
        }
    }
}
