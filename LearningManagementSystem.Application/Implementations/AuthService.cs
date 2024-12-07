using AutoMapper;
using Hangfire;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Parent;
using LearningManagementSystem.Application.Dtos.Teacher;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Extensions;
using LearningManagementSystem.Application.Helpers.Enums;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Application.Settings;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;


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
        public AuthService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ITokenService tokenService, ApplicationDbContext context, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IHttpContextAccessor contextAccessor)
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
           
        }

        public async Task<UserGetDto> RegisterForStudent(RegisterDto registerDto)
        {
            var appUser = await CreateUser(registerDto);

            await _userManager.AddToRoleAsync(appUser, RolesEnum.Student.ToString());
           var Student = new Student();
            Student.AvarageScore= null;
            Student.AppUserId=appUser.Id;
            await _unitOfWork.StudentRepository.Create(Student);
           await _unitOfWork.Commit();
            var ExistedRequestRegister = await _unitOfWork.RequstToRegisterRepository.GetEntity(s => s.Email == appUser.Email);
            if (ExistedRequestRegister != null)
            {
                string body;
                using (StreamReader sr = new StreamReader("wwwroot/templates/SendingAccountInformation.html"))
                {
                    body = sr.ReadToEnd();
                }
                body = body.Replace("{{UserName}}", appUser.UserName).Replace("{{Password}}",registerDto.Password)
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
            var MappedUser = _mapper.Map<UserGetDto>(appUser);
            return MappedUser;
        }

        public async Task<UserGetDto> RegisterForTeacher(TeacherRegistrationDto teacherRegistrationDto)
        {
          var appUser= await CreateUser(teacherRegistrationDto.Register);
            await _userManager.AddToRoleAsync(appUser, RolesEnum.Teacher.ToString());
            teacherRegistrationDto.Teacher.AppUserId=appUser.Id;
            var MappedTeacher = _mapper.Map<Teacher>(teacherRegistrationDto.Teacher);
            await _unitOfWork.TeacherRepository.Create(MappedTeacher);
            await _unitOfWork.Commit();
            var MappedUser = _mapper.Map<UserGetDto>(appUser);
            return MappedUser;
        }
        public async Task<UserGetDto> RegisterForParent(ParentRegisterDto  parentRegisterDto)
        {
            var appUser = await CreateUser(parentRegisterDto.Register);
            await _roleManager.CreateAsync(new IdentityRole(RolesEnum.Parent.ToString()));

            await _userManager.AddToRoleAsync(appUser, RolesEnum.Parent.ToString());
            parentRegisterDto.Parent.AppUserId=appUser.Id;
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
                        throw new CustomException(400, "StudentId", "the choosen student  doesnt exist");

                    }
                }
                MappedParent.Students = Students;
            }
            
            await _unitOfWork.ParentRepository.Create(MappedParent);
            await _unitOfWork.Commit();
            var MappedUser = _mapper.Map<UserGetDto>(appUser);
            return MappedUser;
        }
        private async Task<AppUser> CreateUser(RegisterDto registerDto)
        {
            var existUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) throw new CustomException(400, "UserName", "UserName is already Taken");
            var existUserEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existUserEmail != null) throw new CustomException(400, "Email", "Email is already taken");
            if (await _context.Users.FirstOrDefaultAsync(s => s.PhoneNumber.ToLower() == registerDto.PhoneNumber.ToLower()) is not null)
            {
                throw new CustomException(400, "PhoneNumber", "PhoneNumber already exists ");

            }
            if (DateTime.Now.Year - registerDto.BirthDate.Year <15)
            {
                throw new CustomException(400, "BirthDate", "Student can not be younger than 15  ");

            }
            AppUser appUser = new AppUser();
            appUser.UserName = registerDto.UserName;
            appUser.Email = registerDto.Email;
            appUser.fullName = registerDto.FullName;
            appUser.PhoneNumber = registerDto.PhoneNumber;

            appUser.Image = "user-profile-icon-vector-avatar-600nw-2247726673.webp";

            appUser.CreatedTime = DateTime.UtcNow;
            appUser.BirthDate = registerDto.BirthDate;
            appUser.IsFirstTimeLogined=true;
            appUser.IsReportedHighly=false;
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
          
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.ToDictionary(e => e.Code, e => e.Description);

                throw new CustomException(400, errorMessages);
            }
            return appUser;
        }
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var User = await _userManager.Users.Include(s => s.Reports).FirstOrDefaultAsync(s => s.Email.ToLower() == loginDto.UserNameOrGmail.ToLower());
            if (User == null)
            {
                User = await _userManager.Users.Include(s => s.Reports).FirstOrDefaultAsync(s => s.UserName.ToLower() == loginDto.UserNameOrGmail.ToLower());
                if (User == null)
                {
                    throw new CustomException(400, "UserNameOrGmail", "userName or email is wrong\"");
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
                throw new CustomException(400, "Password", "Password or email is wrong\"");
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
                  
                   
                    throw new CustomException(403, "UserNameOrGmail", $"you are blocked until {User.BlockedUntil?.ToString("dd MMM yyyy hh:mm")}");
                }
            }
            if (User.Reports.Count >= 6)
            {
                throw new CustomException(400, "Report", "your account is reported too many times");
            }

            IList<string> roles = await _userManager.GetRolesAsync(User);
            var Audience = _jwtSettings.Audience;
            var SecretKey = _jwtSettings.secretKey;
            var Issuer = _jwtSettings.Issuer;
            return new AuthResponseDto
            {
                IsSuccess = true,
                Token = tokenService.GetToken(SecretKey, Audience, Issuer, User, roles)
            };
                
        }
        public async Task<string> UpdateImage(UserUpdateImageDto userUpdateImageDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new CustomException(403, "this user doesnt exist");
            if (!string.IsNullOrEmpty(user.Image))
            {
                user.Image.DeleteFile();
            }
            user.Image = userUpdateImageDto.Image.Save(Directory.GetCurrentDirectory(), "img");
            await _userManager.UpdateAsync(user);
            return user.Image;
        }
        public async Task<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new CustomException(404, "Id", "User  not found");
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.ToDictionary(e => e.Code, e => e.Description);
                throw new CustomException(400, errorMessages);
            }
            return result.ToString();
        }
        public async Task<ResetPasswordEmailDto> ResetPasswordSendEmail(ResetPasswordEmailDto resetPasswordEmailDto)
        {
            if (string.IsNullOrEmpty(resetPasswordEmailDto.Email))
            {
                throw new CustomException(400, "Email is required.");
            }
            var user = await GetUserByEmailAsync(resetPasswordEmailDto.Email);
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetPasswordEmailDto.Token = token;
            return resetPasswordEmailDto;
        }
        public async Task<string> ResetPassword(string email, string token,ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new CustomException(400,"Email", "Email is required.");
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new CustomException(400,"token", "token is reqeuired.");
            }
            await CheckExperySutiationOfToken(email, token);
            var existedUser = await GetUserByEmailAsync(email);
            var isNewOrCurrentPassword = await _userManager.CheckPasswordAsync(existedUser, resetPasswordDto.Password);
            if (isNewOrCurrentPassword)
            {
                throw new CustomException(400,"Password", "You cannot use your previous password.");
            }
            var result = await _userManager.ResetPasswordAsync(existedUser, token, resetPasswordDto.Password);
            if (!result.Succeeded) throw new CustomException(400, result.Errors.ToString());
            await _userManager.UpdateSecurityStampAsync(existedUser);
            return "password Reseted";

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
        public async Task<string> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)|| id is null)
            {
                throw new CustomException(400, "Id", "Id can not be null");

            }
            var existedUser = await _userManager.Users
     .Include(u => u.Teacher) 
     .FirstOrDefaultAsync(u => u.Id == id);
            if (existedUser is null)
            {
                throw new CustomException(400, "User", "User can not be null");
            }
            if(existedUser.Teacher is not null)
            {
                var existedTeacher = await _unitOfWork.TeacherRepository.GetEntity(s => s.AppUserId == existedUser.Id);
                if(existedTeacher is null)
                {
                    throw new CustomException(400, "Teacher", "Teacher can not be null");

                }
                await _unitOfWork.TeacherRepository.Delete(existedTeacher);
                await _unitOfWork.Commit();
            }
            await _userManager.DeleteAsync(existedUser);
            return existedUser.Id;
        }
        private async Task<AppUser> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new CustomException(400, "Email is required.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new CustomException(404, "User not found.");
            }
            return user;
        }
        public async Task<string> GetUserName()
        {
            var user = await GetUserWithIdInTheSystem();
            return user.UserName;
        }
        public async Task<UserGetDto> Profile()
        {
        var existedUser = await GetUserWithIdInTheSystem();
            var mappedUser=_mapper.Map<UserGetDto>(existedUser);
            return mappedUser;
        }
        private async Task<AppUser> GetUserWithIdInTheSystem()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException(400, "Id", "User ID cannot be null");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                if (user is null) throw new CustomException(403, "this user doesnt exist");
            }
            return user;    

        }
    }
}
