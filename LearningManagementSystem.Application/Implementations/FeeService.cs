using AutoMapper;
using CloudinaryDotNet;
using LearningManagementSystem.Application.Dtos.Ai;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Helpers.Enums;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
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
                return Result<string>.Failure("StudentId", "Id is invalid",null, ErrorType.NotFoundError);
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
            
                var allStudents = await _unitOfWork.StudentRepository.GetAll(s => s.IsEnrolled == true && !s.IsDeleted, true, includes: new Func<IQueryable<Student>, IQueryable<Student>>[]
            {
                query => query.Include(s=>s.courseStudents).ThenInclude(s=>s.Course)
            });

            foreach (var student in allStudents)
            {
                decimal amount = student.courseStudents.Where(x => x.Course != null).Select(x => x.Course.Price).Sum();
                var hasFeeInThisMonth = await _unitOfWork.FeeRepository.isExists(s => s.Id == student.Id && s.CreatedTime.Value.Year == DateTime.UtcNow.Year &&
                 s.CreatedTime.Value.Month == DateTime.UtcNow.Month);
                if (!hasFeeInThisMonth)
                {
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
                return Result<string>.Failure("UserId", "User ID cannot be null",null, ErrorType.UnauthorizedError);
            }
            var existedUser = await _userManager.Users
     .Include(u => u.Student)
       .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser == null)
            {
                return Result<string>.Failure("User", "User  cannot be null or not  found", null, ErrorType.UnauthorizedError);
            }
            var existedFee=await _unitOfWork.FeeRepository.GetEntity(s=>s.Id==feeImageUploadDto.Id&&s.StudentId==existedUser.Student.Id&&!s.IsDeleted);
            if (existedFee == null)
            {
                return Result<string>.Failure("Fee", "Fee  cannot be null or not  found", null, ErrorType.NotFoundError);
            }
            if (existedFee.PaymentStatus == PaymentStatus.Paid)
            {
                return Result<string>.Failure("Fee", "Fee  already paid", null, ErrorType.BusinessLogicError);
            }
            existedFee.PaymentMethod=Core.Entities.PaymentMethod.BankTransfer;
            existedFee.ProvementImageUrl= await _photoOrVideoService.UploadMediaAsync(feeImageUploadDto.image, false);
            await _unitOfWork.FeeRepository.Update(existedFee);
            await _unitOfWork.Commit();
            return Result<string>.Success("yout request is accpeted , now our admins will check your banktransfer image");
        }
        public async Task<Result<string>> VerifyFee(Guid id)
        {
            if (id == Guid.Empty)
            {
              return  Result<string>.Failure(null, "Invalid GUID provided.", null, ErrorType.ValidationError);
            }
            var existedFee=await _unitOfWork.FeeRepository.GetEntity(s=>s.Id==id&!s.IsDeleted);
            if (existedFee is null) return Result<string>.Failure(null, "fee is null", null, ErrorType.NotFoundError);
            if (existedFee.ProvementImageUrl != null && existedFee.IsBankTransferAccepted)
                return Result<string>.Failure("Fee", "Fee already acceppted",null, ErrorType.BusinessLogicError);
            existedFee.PaymentStatus=PaymentStatus.Paid;
            existedFee.IsBankTransferAccepted=true;
            existedFee.PaidDate=DateTime.Now;
            await _unitOfWork.FeeRepository.Update(existedFee);
            await _unitOfWork.Commit();
            return Result<string>.Success($"this {id} is accepted");
        }
        public async Task<Result<FeeResponseDto>> ProcessPayment(Guid id,FeeHandleDto feeHandleDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (id == Guid.Empty)
                {
                    return Result<FeeResponseDto>.Failure(null, "Invalid GUID provided.",null, ErrorType.ValidationError);
                }
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Result<FeeResponseDto>.Failure("UserId", "User ID cannot be null", null, ErrorType.UnauthorizedError);
                }
                var existedUser = await _userManager.Users
         .Include(u => u.Student)
           .FirstOrDefaultAsync(u => u.Id == userId);
                if (existedUser == null)
                {
                    return Result<FeeResponseDto>.Failure("User", "User  cannot be null or not  found",null, ErrorType.UnauthorizedError);
                }
                var rolesOfExistedUser =await _userManager.GetRolesAsync(existedUser);

                if(existedUser.Student is null||!rolesOfExistedUser.Contains(RolesEnum.Student.ToString()))
                {
                    return Result<FeeResponseDto>.Failure("User", "User  is not student", null, ErrorType.UnauthorizedError);
                }
                var existedFee = await _unitOfWork.FeeRepository.GetEntity(s => s.Id == id && s.StudentId == existedUser.Student.Id && !s.IsDeleted);
                if (existedFee == null)
                {
                    return Result<FeeResponseDto>.Failure("Fee", "Fee  cannot be null or not  found",null, ErrorType.NotFoundError);
                }
                if (existedFee.PaymentStatus == PaymentStatus.Paid)
                {
                    return Result<FeeResponseDto>.Failure("Fee", "Fee  already paid", null, ErrorType.BusinessLogicError);
                }
                var paymentIntentOptions = new PaymentIntentCreateOptions
                {
                    Amount = (long)(existedFee.Amount * 100), 
                    Currency = "usd",
                    Customer = existedUser.CustomerId != null ? existedUser.CustomerId : null, 
                    PaymentMethodTypes = new List<string> { "card" },
                    PaymentMethod = feeHandleDto.PaymentMethodId, 
                    Confirm = true
                };
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);
                if (paymentIntent.Status == "succeeded")
                {
                    existedFee.PaymentStatus = PaymentStatus.Paid;
                    existedFee.PaidDate = DateTime.Now;
                    existedFee.PaymentMethod = Core.Entities.PaymentMethod.CreditCard;
                    existedFee.PaymentReference = paymentIntent.ClientSecret;
                    existedFee.Description= feeHandleDto.Description;
                    await _unitOfWork.FeeRepository.Update(existedFee);
                    await _unitOfWork.Commit();
                    var mappedUser = _mapper.Map<AppUserInFee>(existedUser);
                    return Result<FeeResponseDto>.Success(new FeeResponseDto
                    {
                        Amount = existedFee.Amount,
                        Currency = "usd",
                        Customer = mappedUser,
                        clientSecret = paymentIntent.ClientSecret,
                        Message = "Payment successful and fee status updated."
                    });
                }
                else if (paymentIntent.Status == "requires_action")
                {
                    return Result<FeeResponseDto>.Failure(null, "Further authentication required", null, ErrorType.BusinessLogicError);
                }
                else
                {
                    return Result<FeeResponseDto>.Failure(null, "Payment was not successful.", null, ErrorType.SystemError);
                }
            }
            catch (StripeException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                var message = $"Exception message: {ex.Message}, Inner: {ex.InnerException?.Message ?? "None"}";
                return Result<FeeResponseDto>.Failure(null, message, null, ErrorType.SystemError);
            }
            catch (Exception ex)
            {
                var message = $"Exception message: {ex.Message}, Inner: {ex.InnerException?.Message ?? "None"}";

                await _unitOfWork.RollbackTransactionAsync();
                throw new CustomException(500, "Fee error", message);
            }
        }
       public async Task<Result<bool>> IsFeePaid(string userId)
        {
           
            var existedUser = await _userManager.Users
     .Include(u => u.Student)
       .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser == null)
            {
                return Result<bool>.Failure("User", "User  cannot be null or not  found", null, ErrorType.UnauthorizedError);
            }
            var rolesOfExistedUser = await _userManager.GetRolesAsync(existedUser);

            if (existedUser.Student is null || !rolesOfExistedUser.Contains(RolesEnum.Student.ToString()))
            {
                return Result<bool>.Failure("User", "User  is not student",null, ErrorType.UnauthorizedError);
            }
            var LatestFee = await _unitOfWork.FeeRepository.GetLaastFeeAsync(s=>s.StudentId==existedUser.Student.Id);
            if(LatestFee == null) return Result<bool>.Failure("Fee", "Fee  cannot be null or not  found",null, ErrorType.NotFoundError);
            if(LatestFee.PaymentStatus==PaymentStatus.Paid)
              return  Result<bool>.Success(true);
            else
                return Result<bool>.Success(false);
          

        }
        public async Task<Result<PaginationDto<FeeListItemDto>>> GetAllOfUsersFees(DateTime? startPaidDate=null, DateTime? endPaidDateTime=null, int pageNumber = 1,
           int pageSize = 10)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Result<PaginationDto<FeeListItemDto>>.Failure("UserId", "User ID cannot be null",null, ErrorType.ValidationError);
            }

            var feesQuery = await _unitOfWork.FeeRepository.GetQuery(s => s.Student.AppUserId == userId && s.IsDeleted == false, includes: new Func<IQueryable<Fee>, IQueryable<Fee>>[]
            {
                query => query.Include(s=>s.Student)
            });
            if (startPaidDate != null || endPaidDateTime != null)
                feesQuery = feesQuery.Where(o => (startPaidDate == null || o.PaidDate >= startPaidDate) &&
                (endPaidDateTime == null || o.PaidDate <= endPaidDateTime));
            feesQuery = feesQuery.OrderByDescending(o => o.PaidDate);
            var paginatedResult = await PaginationDto<FeeListItemDto>.Create(
                feesQuery.Select(f => _mapper.Map<FeeListItemDto>(f)), pageNumber, pageSize);
            return Result<PaginationDto<FeeListItemDto>>.Success(paginatedResult);

        }
        public async Task<Result<FeeReturnDto>> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Result<FeeReturnDto>.Failure(null, "Invalid GUID provided.",null, ErrorType.ValidationError);
            }
            var existedFee = await _unitOfWork.FeeRepository.GetEntity(s => s.Id == id & !s.IsDeleted);
            if (existedFee is null) return Result<FeeReturnDto>.Failure(null, "fee is null", null, ErrorType.NotFoundError);
            var mappedFee=_mapper.Map<FeeReturnDto>(existedFee);
            return Result<FeeReturnDto>.Success(mappedFee);
        }
    }
}
