using AutoMapper;
using Hangfire;
using LearningManagementSystem.Application.Dtos.Report;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningManagementSystem.Application.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _userManager = userManager;
            _emailService = emailService;
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
                throw new CustomException(404, "User", "User  cannot be null");
            }
            reportCreateDto.AppUserId = userId;
            var ReportedUser = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == reportCreateDto.ReportedUserId);
            if (ReportedUser is null)
            {
                throw new CustomException(400, "ReportedUserId", "Reported user not found");
            }
            if (ReportedUser.Id == existedUser.Id)
            {
                throw new CustomException(404, "User", "User  cannot report himself");
            }
            if (await _unitOfWork.ReportOptionRepository.isExists(s => s.Id != reportCreateDto.ReportOptionId))
            {
                throw new CustomException(400, "ReportOptionId", "ReportOption  is invalid");
            }
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var userReports = existedUser.Reports.ToList();
            var count = existedUser.Reports.Count(s => s.CreatedTime >= today && s.CreatedTime < tomorrow);
          
            if (count >= 3)
            {
                throw new CustomException(400, "Reports", "amount of reports you can have reached limit");
            }
            var mappedReport = _mapper.Map<Report>(reportCreateDto);
            await _unitOfWork.ReportRepository.Create(mappedReport);
            await _unitOfWork.Commit();
            var countOfReportedUser=await _unitOfWork.ReportRepository.GetAll(s=>s.ReportedUserId==ReportedUser.Id);
            if (countOfReportedUser.Count() >= 3 && !ReportedUser.IsReportedHighly)
            {
                ReportedUser.IsReportedHighly = true;
                await  _userManager.UpdateAsync(ReportedUser);
            }
            var IncludedMappedReport=await _unitOfWork.ReportRepository.GetEntity(s=>s.Id==mappedReport.Id, includes: new Func<IQueryable<Report>, IQueryable<Report>>[]
            {
                query => query.Include(s=>s.ReportedUser).Include(s=>s.ReportOption)
            });    
            var MappedReturnedReportDto = _mapper.Map<ReportReturnDto>(IncludedMappedReport);
            return MappedReturnedReportDto;
        }
        public async Task<string> VerifyReport(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(440, "Invalid GUID provided.");
            }
            var existedReport = await _unitOfWork.ReportRepository.GetEntity(s => s.Id == id && s.IsDeleted == false, includes: new Func<IQueryable<Report>, IQueryable<Report>>[] {
                 query => query
            .Include(p => p.AppUser)
            });

            if (existedReport == null)
            {
                throw new CustomException(400, "existedReport", "existedReport  not found");
            }
            if (!existedReport.IsVerified)
            {
                existedReport.IsVerified = true;
                await _unitOfWork.ReportRepository.Update(existedReport);
                await _unitOfWork.Commit();
            }
            var body = "<h1>Welcome!</h1><p>Report already saved and accepted. We're excited to have you!</p>";

            BackgroundJob.Enqueue(() => _emailService.SendEmail(
                   "nihadcoding@gmail.com",
                   existedReport.AppUser.Email,
                   "Report already saved and accepted",
                   body,
                   "smtp.gmail.com",
                   587,
                   true,
                   "nihadcoding@gmail.com",
                   "gulzclohfwjelppj"
               ));
            return "Approved By Admin";
        }
        public async Task<string> DeleteForUser(Guid id)
        {
            var userReport = await GetUserReport(id);
            userReport.IsDeleted = true;
            await _unitOfWork.ReportRepository.Update(userReport);
            await _unitOfWork.Commit();
            return "Deleted";
        }
        public async Task<string> DeleteForAdmin(Guid id)
        {
         var userReport=   await GetUserReport(id);
            await _unitOfWork.ReportRepository.Delete(userReport);  
            await _unitOfWork.Commit();
            return "Deleted By Admin";
        }
        private async Task<Report> GetUserReport(Guid id)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            if (id == Guid.Empty)
            {
                throw new CustomException(440, "Invalid GUID provided.");
            }
            var existedReport = await _unitOfWork.ReportRepository.GetEntity(s => s.Id == id && s.AppUserId == userId && s.IsDeleted == false);
            if (existedReport is null) throw new CustomException(400, "existedReport", "existedReport  not found");
            return existedReport;
        }
    }
}
