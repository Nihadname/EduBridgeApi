﻿using AutoMapper;
using LearningManagementSystem.Application.Dtos.Address;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Dtos.Parent;
using LearningManagementSystem.Application.Dtos.Report;
using LearningManagementSystem.Application.Dtos.ReportOption;
using LearningManagementSystem.Application.Dtos.RequstToRegister;
using LearningManagementSystem.Application.Dtos.Teacher;
using LearningManagementSystem.Application.Interfaces;
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
            var url = _contextAccessor.HttpContext?.Request.Host.HasValue ?? false
      ? new UriBuilder(_contextAccessor.HttpContext.Request.Scheme,
                       _contextAccessor.HttpContext.Request.Host.Host,
                       _contextAccessor.HttpContext.Request.Host.Port ?? 80).Uri.AbsoluteUri
      : "https://defaulturl.com/";
            CreateMap<AppUser, UserGetDto>()
             .ForMember(s => s.PhoneNumber, map => map.MapFrom(d => d.PhoneNumber));
            CreateMap<TeacherCreateDto, Teacher>();
            CreateMap<ParentCreateDto, Parent>();
            CreateMap<RequstToRegisterCreateDto, RequestToRegister>();
            CreateMap<Course, CourseSelectItemDto>();
            CreateMap<Course, CourseCreateOrUpdateReturnDto>();
                
            CreateMap<CourseCreateDto, Course>();
                 


            CreateMap<CourseUpdateDto, Course>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Note, NoteReturnDto>();
            CreateMap<Note, NoteListItemDto>();
            CreateMap<NoteCreateDto, Note>();
            CreateMap<NoteUpdateDto, Note>()
                            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ReportCreateDto, Report>();
            CreateMap<AppUser, UserReportReturnDto>();
            CreateMap<ReportOption, ReportOptionInReportReturnDto>();
            CreateMap<Report, ReportReturnDto>()
                               .ForMember(s => s.userReportReturnDto, map => map.MapFrom(d => d.AppUser))
                               .ForPath(s => s.optionInReportReturnDto.Id, map => map.MapFrom(d => d.ReportOption.Id))
                               .ForPath(s => s.optionInReportReturnDto.Name, map => map.MapFrom(d => d.ReportOption.Name));
            CreateMap<RequestToRegister, RequestToRegisterListItemDto>();
            CreateMap<ReportOptionCreateDto, ReportOption>();
            CreateMap<ReportOption, ReportOptionReturnDto>();
            CreateMap<Course, CourseListItemDto>();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AppUserInAdress, AppUser>();
            CreateMap<Address, AddressReturnDto>();
            CreateMap<Lesson,LessonInCourseReturnDto>();
            CreateMap<Course, CourseReturnDto>()
                .ForMember(s => s.Lessons, map => map.MapFrom(d => d.lessons));
            CreateMap<FeeCreateDto, Fee>();
            CreateMap<AppUser, AppUserInFee>();
            CreateMap<Fee, FeeListItemDto>();
            CreateMap<Fee, FeeReturnDto>();
            CreateMap<Address, AddressListItemDto>();
        }
    }
}
