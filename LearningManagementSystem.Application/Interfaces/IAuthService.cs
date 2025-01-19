using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Parent;
using LearningManagementSystem.Application.Dtos.Teacher;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserGetDto>> RegisterForStudent(RegisterDto registerDto);
        Task<Result<UserGetDto>> RegisterForTeacher(TeacherRegistrationDto teacherRegistrationDto);
        Task<Result<AuthResponseDto>> Login(LoginDto loginDto);
        Task<Result<UserGetDto>> RegisterForParent(ParentRegisterDto parentRegisterDto);
        Task<string> UpdateImage(UserUpdateImageDto userUpdateImageDto);
        Task<string> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<ResetPasswordEmailDto> ResetPasswordSendEmail(ResetPasswordEmailDto resetPasswordEmailDto);
        Task<string> ResetPassword(string email, string token, ResetPasswordDto resetPasswordDto);
        Task<string> Delete(string id);
        Task<string> GetUserName();
        Task<UserGetDto> Profile();
        Task<Result<string>> SendVerificationCode(string email);
        Task<string> VerifyCode(VerifyCodeDto verifyCodeDto);
    }
}
