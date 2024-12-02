using AutoMapper;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Dtos.Parent;
using LearningManagementSystem.Application.Dtos.RequstToRegister;
using LearningManagementSystem.Application.Dtos.Teacher;
using LearningManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Profiles
{
    public class MapperProfile:Profile
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public MapperProfile(IHttpContextAccessor contextAccessor)
        {
      
            _contextAccessor = contextAccessor;
            CreateMap<AppUser, UserGetDto>();
            CreateMap<TeacherCreateDto, Teacher>();
            CreateMap<ParentCreateDto, Parent>();
            CreateMap<RequstToRegisterCreateDto, RequestToRegister>();
            CreateMap<Course,CourseSelectItemDto>();
            CreateMap<Course, CourseReturnDto>();
            CreateMap<CourseCreateDto, Course>();
            CreateMap<CourseUpdateDto, Course>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Note, NoteReturnDto>();
            CreateMap<Note,NoteListItemDto>();
            CreateMap<NoteCreateDto, Note>();
            CreateMap<NoteUpdateDto, Note>()
                            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
