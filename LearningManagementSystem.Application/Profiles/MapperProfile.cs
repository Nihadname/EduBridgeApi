using AutoMapper;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Parent;
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
        }
    }
}
