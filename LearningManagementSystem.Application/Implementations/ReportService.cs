using AutoMapper;
using CloudinaryDotNet.Actions;
using Hangfire;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Report;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
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
        public async Task<Result<ReportReturnDto>> Create(ReportCreateDto reportCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<ReportReturnDto>.Failure("Id", "User ID cannot be null", null, ErrorType.UnauthorizedError);
            }
            var existedUser = await _userManager.Users
                 .Include(u => u.Reports)
                 .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser is null)
            {
                return Result<ReportReturnDto>.Failure("User", "User  cannot be null",null, ErrorType.NotFoundError);
            }
            reportCreateDto.AppUserId = userId;
            var ReportedUser = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == reportCreateDto.ReportedUserId);
            if (ReportedUser is null)
            {
                return Result<ReportReturnDto>.Failure("ReportedUserId", "Reported user not found",null, ErrorType.NotFoundError);
            }
            if (ReportedUser.Id == existedUser.Id)
            {
                return Result<ReportReturnDto>.Failure("User", "User  cannot report himself", null, ErrorType.BusinessLogicError);
            }
            if (await _unitOfWork.ReportOptionRepository.isExists(s => s.Id != reportCreateDto.ReportOptionId))
            {
                return Result<ReportReturnDto>.Failure("ReportOptionId", "ReportOption is invalid",null, ErrorType.ValidationError);
            }
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var userReports = existedUser.Reports.ToList();
            var count = existedUser.Reports.Count(s => s.CreatedTime >= today && s.CreatedTime < tomorrow);
          
            if (count >= 3)
            {
                return Result<ReportReturnDto>.Failure("Reports", "amount of reports you can have reached limit",null, ErrorType.BusinessLogicError);
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
            return Result<ReportReturnDto>.Success(MappedReturnedReportDto);
        }
        public async Task<Result<string>> VerifyReport(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Result<string>.Failure(null, "Invalid GUID provided.",null, ErrorType.ValidationError);
            }
            var existedReport = await _unitOfWork.ReportRepository.GetEntity(s => s.Id == id && s.IsDeleted == false, includes: new Func<IQueryable<Report>, IQueryable<Report>>[] {
                 query => query
            .Include(p => p.AppUser)
            });

            if (existedReport == null)
            {
                return Result<string>.Failure("ReportedUserId", "Reported user not found",null, ErrorType.NotFoundError);
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
            return Result<string>.Success("Approved By Admin");
        }
        public async Task<Result<string>> DeleteForUser(Guid id)
        {
            var userReportResult = await GetUserReport(id);
            if (!userReportResult.IsSuccess) return Result<string>.Failure(userReportResult.ErrorKey, userReportResult.Message,userReportResult.Errors, (ErrorType)userReportResult.ErrorType);
            userReportResult.Data.IsDeleted = true;
            await _unitOfWork.ReportRepository.Update(userReportResult.Data);
            await _unitOfWork.Commit();
            return Result<string>.Success("Deleted");
        }
        public async Task<Result<string>> DeleteForAdmin(Guid id)
        {
         var userReportResult=   await GetUserReport(id);
            if (!userReportResult.IsSuccess) return Result<string>.Failure(userReportResult.ErrorKey, userReportResult.Message, userReportResult.Errors, (ErrorType)userReportResult.ErrorType);

            await _unitOfWork.ReportRepository.Delete(userReportResult.Data);  
            await _unitOfWork.Commit();
            return Result<string>.Success("Deleted by Admin");
        }
        private async Task<Result<Report>> GetUserReport(Guid id)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<Report>.Failure("Id", "User ID cannot be null", null, ErrorType.UnauthorizedError);
            }
            if (id == Guid.Empty)
            {
                return Result<Report>.Failure("Id", "Invalid GUID provided.",null, ErrorType.ValidationError);

            }
            var existedReport = await _unitOfWork.ReportRepository.GetEntity(s => s.Id == id && s.AppUserId == userId && s.IsDeleted == false);
            if (existedReport is null) throw new CustomException(400, "existedReport", "existedReport  not found");
            return Result<Report>.Success(existedReport);
        }
    }
}
