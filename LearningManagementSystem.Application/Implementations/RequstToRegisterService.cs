using AutoMapper;
using LearningManagementSystem.Application.Dtos.RequstToRegister;
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
    public class RequstToRegisterService: IRequstToRegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public RequstToRegisterService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Create(RequstToRegisterCreateDto requstToRegisterCreateDto)
        {
         
                if (!await _unitOfWork.CourseRepository.isExists(s => s.Name == requstToRegisterCreateDto.ChoosenCourse))
                {
                    throw new CustomException(400, "Course", "You have choosen an invalid course");
                }
                List<string> allCoursesNames = (await _unitOfWork.CourseRepository.GetAll()).Select(s => s.Name).ToList();
                requstToRegisterCreateDto.ExistedCourses = allCoursesNames;
                if (requstToRegisterCreateDto.IsParent is true)
                {
                    if (string.IsNullOrWhiteSpace(requstToRegisterCreateDto.ChildName))
                    {
                        throw new CustomException(400, "Parent", "You identify as a parent so , you have to mention name of your child");

                    }
                    if (requstToRegisterCreateDto.ChildAge is null || !requstToRegisterCreateDto.ChildAge.HasValue)
                    {
                        throw new CustomException(400, "Parent", "You identify as a parent so , you have to mention age of your child");
                    }
                }
                var MappedRequestRegister = _mapper.Map<RequestToRegister>(requstToRegisterCreateDto);
                await _unitOfWork.RequstToRegisterRepository.Create(MappedRequestRegister);
                await _unitOfWork.Commit();


                return "Succesfully send your reuqest";
            
        }
    }
}
