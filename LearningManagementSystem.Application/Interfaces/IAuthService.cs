using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Parent;
using LearningManagementSystem.Application.Dtos.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IAuthService
    {
         Task<UserGetDto> RegisterForStudent(RegisterDto registerDto);
        Task<UserGetDto> RegisterForTeacher(TeacherRegistrationDto teacherRegistrationDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<UserGetDto> RegisterForParent(ParentRegisterDto parentRegisterDto);
        Task<string> UpdateImage(UserUpdateImageDto userUpdateImageDto);
        Task<string> ChangePassword(ChangePasswordDto changePasswordDto);
    }
}
