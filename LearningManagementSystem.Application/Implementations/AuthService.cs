using AutoMapper;
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
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class AuthService : IAuthService
    {
        private UserManager<AppUser> _userManager;
        
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService tokenService;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ITokenService tokenService, ApplicationDbContext context, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            this.tokenService = tokenService;
            _context = context;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
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

            appUser.Image = null;

            appUser.CreatedTime = DateTime.UtcNow;
            appUser.BirthDate = registerDto.BirthDate;
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
            var User = await _userManager.FindByEmailAsync(loginDto.UserNameOrGmail);
            if (User == null)
            {
                User = await _userManager.FindByNameAsync(loginDto.UserNameOrGmail);
                if (User == null)
                {
                    throw new CustomException(400, "UserNameOrGmail", "userName or email is wrong\"");
                }
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
                    // User is still blocked
                   
                    throw new CustomException(403, "UserNameOrGmail", $"you are blocked until {User.BlockedUntil?.ToString("dd MMM yyyy hh:mm")}");
                }
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
    }
}
