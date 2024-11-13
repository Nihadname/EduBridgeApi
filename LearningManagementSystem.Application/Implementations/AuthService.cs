using AutoMapper;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Helpers.Enums;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Application.Settings;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public AuthService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ITokenService tokenService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            this.tokenService = tokenService;
            _context = context;
        }

        public async Task<UserGetDto> RegisterForStudent(RegisterDto registerDto)
        {
            var appUser = await CreateUser(registerDto);

            await _userManager.AddToRoleAsync(appUser, RolesEnum.Student.ToString());
            
            var MappedUser = _mapper.Map<UserGetDto>(appUser);
            return MappedUser;
        }

        public async Task<UserGetDto> RegisterForTeacher(RegisterDto registerDto)
        {
          var appUser= await CreateUser(registerDto);
            await _userManager.AddToRoleAsync(appUser, RolesEnum.Teacher.ToString());

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
        public async Task<string> Login(LoginDto loginDto)
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
            return tokenService.GetToken(SecretKey, Audience, Issuer, User, roles);
        }

    }
}
