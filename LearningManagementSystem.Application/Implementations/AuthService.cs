using AutoMapper;
using Hangfire;
using LearningManagementSystem.Application.AppDefaults;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Parent;
using LearningManagementSystem.Application.Dtos.Teacher;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Extensions;
using LearningManagementSystem.Application.Helpers.Enums;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Application.Settings;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Security.Claims;
using ZiggyCreatures.Caching.Fusion;


namespace LearningManagementSystem.Application.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService tokenService;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IFusionCache _cache;
        private readonly IPhotoOrVideoService _photoOrVideoService;
        public AuthService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ITokenService tokenService, ApplicationDbContext context, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IHttpContextAccessor contextAccessor, IFusionCache cache, IPhotoOrVideoService photoOrVideoService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            this.tokenService = tokenService;
            _context = context;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _cache = cache;
            _photoOrVideoService = photoOrVideoService;
        }

        public async Task<Result<UserGetDto>> RegisterForStudent(RegisterDto registerDto)
        {
            var appUserResult = await CreateUser(registerDto);
            if (!appUserResult.IsSuccess) return Result<UserGetDto>.Failure(appUserResult.ErrorKey, appUserResult.Message,appUserResult.Errors, (ErrorType)appUserResult.ErrorType);

            await _userManager.AddToRoleAsync(appUserResult.Data, RolesEnum.Student.ToString());
            await _userManager.UpdateAsync(appUserResult.Data);
            var Student = new Student();
            Student.AvarageScore= null;
            Student.AppUserId= appUserResult.Data.Id;
            Student.IsEnrolled=false;
            await _unitOfWork.StudentRepository.Create(Student);
           await _unitOfWork.Commit();
           
            var MappedUser = _mapper.Map<UserGetDto>(appUserResult.Data);
            return Result<UserGetDto>.Success(MappedUser);
        }

        public async Task<Result<UserGetDto>> RegisterForTeacher(TeacherRegistrationDto teacherRegistrationDto)
        {
            var appUserResult = await CreateUser(teacherRegistrationDto.Register);
            if (!appUserResult.IsSuccess) return Result<UserGetDto>.Failure(appUserResult.ErrorKey, appUserResult.Message, appUserResult.Errors, (ErrorType)appUserResult.ErrorType);
            await _userManager.AddToRoleAsync(appUserResult.Data, RolesEnum.Teacher.ToString());
            teacherRegistrationDto.Teacher.AppUserId=appUserResult.Data.Id;
            var MappedTeacher = _mapper.Map<Teacher>(teacherRegistrationDto.Teacher);
            await _unitOfWork.TeacherRepository.Create(MappedTeacher);
            await _unitOfWork.Commit();
            var MappedUser = _mapper.Map<UserGetDto>(appUserResult.Data);
            return Result<UserGetDto>.Success(MappedUser);
        }
        public async Task<Result<UserGetDto>> RegisterForParent(ParentRegisterDto  parentRegisterDto)
        {
            var appUserResult = await CreateUser(parentRegisterDto.Register);
            if (!appUserResult.IsSuccess) return Result<UserGetDto>.Failure(appUserResult.ErrorKey, appUserResult.Message,  appUserResult.Errors, (ErrorType)appUserResult.ErrorType); await _roleManager.CreateAsync(new IdentityRole(RolesEnum.Parent.ToString()));

            await _userManager.AddToRoleAsync(appUserResult.Data, RolesEnum.Parent.ToString());
            parentRegisterDto.Parent.AppUserId=appUserResult.Data.Id;
            var MappedParent = _mapper.Map<Parent>(parentRegisterDto.Parent);

            var Students = new List<Student>();
            if (parentRegisterDto.Parent.StudentIds.Any())
            {
                foreach (var student in parentRegisterDto.Parent.StudentIds)
                {
                   if(await _unitOfWork.StudentRepository.isExists(s => s.Id == student) is not false)
                    {
                        var ExistedStudent = await _unitOfWork.StudentRepository.GetEntity(s => s.Id == student);
                        Students.Add(ExistedStudent);
                    }
                    else
                    {
                        return Result<UserGetDto>.Failure("StudentId", "the choosen student  doesnt exist",null, ErrorType.NotFoundError);
                    }
                }
                MappedParent.Students = Students;
            }
            
            await _unitOfWork.ParentRepository.Create(MappedParent);
            await _unitOfWork.Commit();
            var MappedUser = _mapper.Map<UserGetDto>(appUserResult.Data);
            return Result<UserGetDto>.Success(MappedUser);
        }
        private async Task<Result<AppUser>> CreateUser(RegisterDto registerDto)
        {
            var existUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) return Result<AppUser>.Failure("UserName", "UserName is already Taken", null, ErrorType.BusinessLogicError);
            var existUserEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existUserEmail != null)
                return Result<AppUser>.Failure("Email", "Email is already taken", null, ErrorType.BusinessLogicError);
            if (await _context.Users.FirstOrDefaultAsync(s => s.PhoneNumber.ToLower() == registerDto.PhoneNumber.ToLower()) is not null)
            {
                return Result<AppUser>.Failure("PhoneNumber", "PhoneNumber already exists", null, ErrorType.BusinessLogicError);
            }
            if (DateTime.Now.Year - registerDto.BirthDate.Year <15)
            {
                return Result<AppUser>.Failure("BirthDate", "Student can not be younger than 15", null, ErrorType.BusinessLogicError);
            }
            AppUser appUser = new AppUser();
            appUser.UserName = registerDto.UserName;
            appUser.Email = registerDto.Email;
            appUser.fullName = registerDto.FullName;
            appUser.PhoneNumber = registerDto.PhoneNumber;
            appUser.Image = AppDefaultValue.DefaultProfileImageUrl;
            appUser.CreatedTime = DateTime.UtcNow;
            appUser.BirthDate = registerDto.BirthDate;
            appUser.IsFirstTimeLogined=true;
            appUser.IsReportedHighly=false;
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
          
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.ToDictionary(e => e.Code, e => e.Description);
                List<string> errors = new List<string>();
                foreach (KeyValuePair<string, string> keyValues in errorMessages)
                {
                    errors.Add(keyValues.Key + " " + keyValues.Value);
                }

                var response = Result<string>.Failure("User  errors found", null, errors, ErrorType.ValidationError);
            }
            var customerOptions = new CustomerCreateOptions
            {
                Email = appUser.Email,
                Name = appUser.UserName
            };
            var service = new CustomerService();
            var stripeCustomer = await service.CreateAsync(customerOptions);
            appUser.CustomerId = stripeCustomer.Id;
            var ExistedRequestRegister = await _unitOfWork.RequstToRegisterRepository.GetEntity(s => s.Email == appUser.Email);
            if (ExistedRequestRegister != null)
            {
                string body;
                using (StreamReader sr = new StreamReader("wwwroot/templates/SendingAccountInformation.html"))
                {
                    body = sr.ReadToEnd();
                }
                body = body.Replace("{{UserName}}", appUser.UserName).Replace("{{Password}}", registerDto.Password)
                    .Replace("{{Email}}", appUser.Email);
                BackgroundJob.Enqueue(() => _emailService.SendEmail(
                   "nihadcoding@gmail.com",
                   appUser.Email,
                   "Account details",
                   body,
                   "smtp.gmail.com",
                   587,
                   true,
                   "nihadcoding@gmail.com",
                   "gulzclohfwjelppj"
               ));
            }
            await SendVerificationCode(appUser.Email);
            return Result<AppUser>.Success(appUser);
        }
        public async Task<Result<string>> SendVerificationCode(string email)
        {
            if (string.IsNullOrEmpty(email)) return Result<string>.Failure("email", "email is null", null, ErrorType.ValidationError);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result<string>.Failure("user", "user is null", null, ErrorType.NotFoundError);
            var verificationCode = new Random().Next(100000, 999999).ToString();
            string salt;
            string hashedCode=verificationCode.GenerateHash(out salt);
            user.VerificationCode = hashedCode;
            user.Salt = salt;
            user.ExpiredDate = DateTime.UtcNow.AddMinutes(10);
            user.IsEmailVerificationCodeValid = false;
            await _userManager.UpdateAsync(user);
            var body = $"<h1>Welcome!</h1><p>Thank you for joining us. We're excited to have you!, this is your verfication code {verificationCode} </p>";
            BackgroundJob.Enqueue(() => _emailService.SendEmail(
                "nihadcoding@gmail.com",
                user.Email,
                "Verify Code",
                body,
                "smtp.gmail.com",
                587,
                true,
                "nihadcoding@gmail.com",
                "gulzclohfwjelppj"
            ));
            return Result<string>.Success("Verification code sent");
        }
        public async Task<Result<string>> VerifyCode(VerifyCodeDto verifyCodeDto)
        {
            var existedUser=await _userManager.FindByEmailAsync(verifyCodeDto.Email);
            if (existedUser is null) return Result<string>.Failure("User", "User is null", null, ErrorType.NotFoundError);
            bool isValid = HashExtension.VerifyHash(verifyCodeDto.Code,existedUser.Salt, existedUser.VerificationCode);
            if (!isValid || existedUser.ExpiredDate < DateTime.UtcNow)
               return Result<string>.Failure("Code", "Invalid or expired verification code.", null, ErrorType.BusinessLogicError);
            existedUser.IsEmailVerificationCodeValid = true;
            existedUser.VerificationCode = null;
            existedUser.ExpiredDate = null;
            await _userManager.UpdateAsync(existedUser);
            return Result<string>.Success("Code verified successfully. You can now log in.");
            }
        public async Task<Result<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var User = await _userManager.Users.Include(s=>s.Student).
                FirstOrDefaultAsync(s=>s.Email.ToLower()==loginDto.UserNameOrGmail.ToLower());
            if (User == null)
            {
                User = await _userManager.Users
                    .Include(s => s.Student)
                    .FirstOrDefaultAsync(s => s.UserName.ToLower() == loginDto.UserNameOrGmail.ToLower());
                if (User == null)
                {
                    return Result<AuthResponseDto>.Failure("UserNameOrGmail", "userName or email is wrong\"",null, ErrorType.NotFoundError);
                }
            }
            if (User.IsFirstTimeLogined)
            {
                User.IsFirstTimeLogined = false;
                await _userManager.UpdateAsync(User);
                var body = "<h1>Welcome!</h1><p>Thank you for joining us. We're excited to have you!</p>"; 
                BackgroundJob.Enqueue(() => _emailService.SendEmail(
                    "nihadcoding@gmail.com", 
                    User.Email,               
                    "Welcome to Our System!",
                    body,                   
                    "smtp.gmail.com",         
                    587,                      
                    true,                     
                    "nihadcoding@gmail.com",  
                    "gulzclohfwjelppj"       
                ));

            }
            var result = await _userManager.CheckPasswordAsync(User, loginDto.Password);

            if (!result)
            {
                return Result<AuthResponseDto>.Failure("Password", "Password or email is wrong\"",null, ErrorType.ValidationError);
            }
            
            if (User.IsBlocked && User.BlockedUntil.HasValue)
            {
                if (User.BlockedUntil.Value <= DateTime.UtcNow)
                {
                    User.IsBlocked = false;
                    User.BlockedUntil = null;
                    await _userManager.UpdateAsync(User);
                }
                else
                {

                    return Result<AuthResponseDto>.Failure("UserNameOrGmail", $"you are blocked until {User.BlockedUntil?.ToString("dd MMM yyyy hh:mm")}",null, ErrorType.BusinessLogicError);
                }
            }
            IList<string> roles = await _userManager.GetRolesAsync(User);

            //if (roles.Contains(RolesEnum.Student.ToString()))
            //{
            //    var latestFee = await _unitOfWork.FeeRepository.GetLaastFeeAsync(
            //        s => s.StudentId == User.Student.Id && !s.IsDeleted
            //    );

            //    if (latestFee is not null && latestFee.PaymentStatus != PaymentStatus.Paid)
            //    {
            //        throw new CustomException(400, "PaymentRequired", "User must pay their fee for this month to log in.");
            //    }
            //}
            if (User.IsReportedHighly)
            {
                return Result<AuthResponseDto>.Failure("User", "You are reported too many times ,so account is locked now, we will contact with you", null, ErrorType.BusinessLogicError);
            }
            if (!User.IsEmailVerificationCodeValid) return Result<AuthResponseDto>.Failure("User", "pls verify your account by getting code",null, ErrorType.BusinessLogicError);
            var Audience = _jwtSettings.Audience;
            var SecretKey = _jwtSettings.secretKey;
            var Issuer = _jwtSettings.Issuer;
            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                IsSuccess = true,
                Token = tokenService.GetToken(SecretKey, Audience, Issuer, User, roles)
            });
                
        }
        public async Task<Result<string>> UpdateImage(UserUpdateImageDto userUpdateImageDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Result<string>.Failure("Id", "User Id cannot be null",null, ErrorType.UnauthorizedError);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<string>.Failure("Id", "this user doesnt exist",null, ErrorType.UnauthorizedError);
            if (!string.IsNullOrEmpty(user.Image))
            {
                user.Image.DeleteFile();
            }
            user.Image =await _photoOrVideoService.UploadMediaAsync(userUpdateImageDto.Image, false);
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(user.Image);
        }
        public async Task<Result<string>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return  Result<string>.Failure("Id", "User ID cannot be null",null, ErrorType.UnauthorizedError);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<string>.Failure("Id", "this user doesnt exist", null, ErrorType.UnauthorizedError);
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.ToDictionary(e => e.Code, e => e.Description);
                throw new CustomException(400, errorMessages);
            }
            return Result<string>.Success(result.ToString());
        }
        public async Task<Result<ResetPasswordEmailDto>> ResetPasswordSendEmail(ResetPasswordEmailDto resetPasswordEmailDto)
        {
            if (string.IsNullOrEmpty(resetPasswordEmailDto.Email))
            {
                return Result<ResetPasswordEmailDto>.Failure(null, "Email is required.",null ,ErrorType.ValidationError);
            }
            var userResult = await GetUserByEmailAsync(resetPasswordEmailDto.Email);
            if (!userResult.IsSuccess) return Result<ResetPasswordEmailDto>.Failure(userResult.ErrorKey, userResult.Message, userResult.Errors, (ErrorType)userResult.ErrorType);
            var token = await _userManager.GeneratePasswordResetTokenAsync(userResult.Data);
            resetPasswordEmailDto.Token = token;
            return Result<ResetPasswordEmailDto>.Success(resetPasswordEmailDto);
        }
        public async Task<Result<string>> ResetPassword(ResetPasswordHandleDto resetPasswordHandleDto)
        {
           
            await CheckExperySutiationOfToken(resetPasswordHandleDto.ResetPasswordTokenAndEmailDto.Email, resetPasswordHandleDto.ResetPasswordTokenAndEmailDto.Token);
            var existedUserResult = await GetUserByEmailAsync(resetPasswordHandleDto.ResetPasswordTokenAndEmailDto.Email);
            if (!existedUserResult.IsSuccess) return Result<string>.Failure(existedUserResult.ErrorKey, existedUserResult.Message, existedUserResult.Errors,(ErrorType)existedUserResult.ErrorType);
            var isNewOrCurrentPassword = await _userManager.CheckPasswordAsync(existedUserResult.Data, resetPasswordHandleDto.resetPasswordDto.Password);
            if (isNewOrCurrentPassword)
            {
             return   Result<string>.Failure("Password", "You cannot use your previous password.",null, ErrorType.BusinessLogicError);
            }
            var result = await _userManager.ResetPasswordAsync(existedUserResult.Data, resetPasswordHandleDto.ResetPasswordTokenAndEmailDto.Token, resetPasswordHandleDto.resetPasswordDto.Password);
            if (!result.Succeeded) throw new CustomException(400, result.Errors.ToString());
            await _userManager.UpdateSecurityStampAsync(existedUserResult.Data);
            return Result<string>.Success("password Reseted");

        }
        public async Task<string> CheckExperySutiationOfToken(string email, string token)
        {
            if (string.IsNullOrEmpty(email))
                throw new CustomException(400, "Email is required.");
            if (string.IsNullOrEmpty(token))
                throw new CustomException(400, "Token is required.");
            var existUser = await _userManager.FindByEmailAsync(email);
            if (existUser == null) throw new CustomException(404, "User is null or empty");
            bool result = await _userManager.VerifyUserTokenAsync(
    existUser,
    _userManager.Options.Tokens.PasswordResetTokenProvider,
    "ResetPassword",
    token
);

            if (!result)
                throw new CustomException(400, "The token is either invalid or has expired.");
            return "hasnt still expired";

        }
        public async Task<Result<string>> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)|| id is null)
            {
                return Result<string>.Failure("Id", "Id can not be null", null, ErrorType.ValidationError);
            }
            var existedUser = await _userManager.Users
     .Include(u => u.Teacher) 
     .FirstOrDefaultAsync(u => u.Id == id);
            if (existedUser is null)
            {
                return Result<string>.Failure("User", "User can not be null", null, ErrorType.NotFoundError);
            }
            if(existedUser.Teacher is not null)
            {
                var existedTeacher = await _unitOfWork.TeacherRepository.GetEntity(s => s.AppUserId == existedUser.Id);
                if(existedTeacher is null)
                {
                    return Result<string>.Failure("User", "User can not be null",null, ErrorType.NotFoundError);
                }
                await _unitOfWork.TeacherRepository.Delete(existedTeacher);
                await _unitOfWork.Commit();
            }
            await _userManager.DeleteAsync(existedUser);
            return Result<string>.Success(existedUser.Id);
        }
        private async Task<Result<AppUser>> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result<AppUser>.Failure(null, "Email is required.",null, ErrorType.ValidationError);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<AppUser>.Failure(null, "User not found.", null, ErrorType.NotFoundError);
            }
            return Result<AppUser>.Success(user);
        }
        public async Task<Result<string>> GetUserName()
        {
            var user =( await GetUserWithIdInTheSystem()).Data;
            return Result<string>.Success(user.UserName);
        }
        public async Task<Result<UserGetDto>> Profile()
        {
            var userResult = await GetUserWithIdInTheSystem();
            if(!userResult.IsSuccess) return Result<UserGetDto>.Failure(userResult.ErrorKey,userResult.Message, userResult.Errors, (ErrorType)userResult.ErrorType);
        var existedUser = userResult.Data;
            
            var mappedUser=_mapper.Map<UserGetDto>(existedUser);
            return Result<UserGetDto>.Success(mappedUser);
        }
        private async Task<Result<AppUser>> GetUserWithIdInTheSystem()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Result<AppUser>.Failure("Id", "User ID cannot be null",null, ErrorType.UnauthorizedError);
            }
            var cacheKey = $"AppUser_{userId}";
            var cachedNote = await _cache.GetOrSetAsync<Result<AppUser>>(cacheKey, async _ =>
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    if (user is null) return Result<AppUser>.Failure("Id", "User ID cannot be null",null, ErrorType.UnauthorizedError);

                }
                return Result<AppUser>.Success(user);
            });
            
            return cachedNote;    

        }
    }
}
