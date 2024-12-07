using AutoMapper;
using LearningManagementSystem.Application.Dtos.Report;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor  _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _userManager = userManager;
        }
        public async Task<ReportReturnDto> Create(ReportCreateDto reportCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            var existedUser = await _userManager.Users
                 .Include(u => u.Reports)
                 .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser is null)
            {
                throw new CustomException(400, "User", "User  cannot be null");
            }
            reportCreateDto.AppUserId = userId;
            if(await _unitOfWork.ReportOptionRepository.isExists(s => s.Id != reportCreateDto.ReportOptionId))
            {
                throw new CustomException(400, "ReportOptionId", "ReportOption  is invalid");
            }
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var count = (await _unitOfWork.ReportRepository.GetAll(s=>s.AppUserId==existedUser.Id && s.CreatedTime >= today && s.CreatedTime < tomorrow)).Count();
            var allReportOptions = await _unitOfWork.ReportRepository.GetAll(s => s.IsDeleted == false);
            if (allReportOptions.Any())
            {
                if (count >= allReportOptions.Count())
                {
                    throw new CustomException(400, "Reports", "amount of reports you can have reached limit");
                }
            }
            if (count > 3)
            {
                throw new CustomException(400, "Reports", "amount of reports you can have reached limit");
            }
            var MappedReport=_mapper.Map<Report>(reportCreateDto);  
            await _unitOfWork.ReportRepository.Create(MappedReport);
            await _unitOfWork.Commit();
            var MappedReturnedReportDto=_mapper.Map<ReportReturnDto>(MappedReport);
            return MappedReturnedReportDto;
        }
    }
}
