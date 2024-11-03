using AutoMapper;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Helpers.Enums;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
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

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
            AppUser appUser = new AppUser();
            appUser.UserName = registerDto.UserName;
            appUser.Email = registerDto.Email;
            appUser.fullName = registerDto.FullName;
            appUser.PhoneNumber = registerDto.PhoneNumber;

            appUser.Image = null;

            appUser.CreatedTime = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.ToDictionary(e => e.Code, e => e.Description);

                throw new CustomException(400, errorMessages);
            }
            return appUser;
        }
    }
}
