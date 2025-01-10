using AutoMapper;
using CloudinaryDotNet;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace LearningManagementSystem.Application.Implementations
{
    public class FeeService :  IFeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoOrVideoService _photoOrVideoService;
        public FeeService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IPhotoOrVideoService photoOrVideoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _photoOrVideoService = photoOrVideoService;
        }
        public async Task<Result<string>> CreateFeeAndAssignToStudent(FeeCreateDto feeCreateDto)
        {
            if (!await _unitOfWork.StudentRepository.isExists(s => s.Id == feeCreateDto.StudentId)) 
                return Result<string>.Failure("StudentId", "Id is invalid", ErrorType.NotFoundError);
            var lastFee = await _unitOfWork.FeeRepository.GetLaastFeeAsync(f => f.StudentId == feeCreateDto.StudentId && !f.IsDeleted);
            feeCreateDto.DueDate = lastFee != null ? lastFee.DueDate.AddMonths(1) : DateTime.Now.AddMonths(1);
            feeCreateDto.PaymentStatus=PaymentStatus.Pending;
            if(feeCreateDto.DiscountPercentage is not null && feeCreateDto.DiscountPercentage > 0)
            {
                feeCreateDto.DiscountedPrice= feeCreateDto.Amount - (feeCreateDto.Amount * feeCreateDto.DiscountPercentage.Value) / 100;
            }
            var mappedFee= _mapper.Map<Fee>(feeCreateDto);
            await _unitOfWork.FeeRepository.Create(mappedFee);
            await _unitOfWork.Commit();
            return Result<string>.Success("fee created for student named");
        }
       public async Task CreateFeeToAllStudents(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var allStudents = await _unitOfWork.StudentRepository.GetAll(s => s.IsEnrolled == true && !s.IsDeleted, true, includes: new Func<IQueryable<Student>, IQueryable<Student>>[]
            {
                query => query.Include(s=>s.courseStudents).ThenInclude(s=>s.Course)
            });

                foreach (var student in allStudents)
                {
                    decimal amount = student.courseStudents.Where(x => x.Course != null).Select(x => x.Course.Price).Sum();
                    FeeCreateDto feeCreateDto = new()
                    {
                        Amount = amount,
                        StudentId = student.Id
                    };
                    await CreateFeeAndAssignToStudent(feeCreateDto);
                }
            }
        }
       public async Task<Result<string>> UploadImageOfBankTransfer(FeeImageUploadDto feeImageUploadDto ) {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<string>.Failure("UserId", "User ID cannot be null", ErrorType.UnauthorizedError);
            }
            var existedUser = await _userManager.Users
     .Include(u => u.Student)
       .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser == null)
            {
                return Result<string>.Failure("User", "User  cannot be null or not  found", ErrorType.UnauthorizedError);
            }
            var existedFee=await _unitOfWork.FeeRepository.GetEntity(s=>s.Id==feeImageUploadDto.Id&&s.StudentId==existedUser.Student.Id&&!s.IsDeleted);
            if (existedFee == null)
            {
                return Result<string>.Failure("Fee", "Fee  cannot be null or not  found", ErrorType.NotFoundError);
            }
            if (existedFee.PaymentStatus == PaymentStatus.Paid)
            {
                return Result<string>.Failure("Fee", "Fee  already paid", ErrorType.BusinessLogicError);
            }
            existedFee.PaymentMethod=PaymentMethod.BankTransfer;
            existedFee.ProvementImageUrl= await _photoOrVideoService.UploadMediaAsync(feeImageUploadDto.image, false);
          return  Result<string>.Success("yout request is accpeted , now our admins will check your banktransfer image");
        }
    }
}
