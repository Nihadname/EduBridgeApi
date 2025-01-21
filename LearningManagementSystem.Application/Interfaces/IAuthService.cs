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
        Task<Result<string>> UpdateImage(UserUpdateImageDto userUpdateImageDto);
        Task<Result<string>> ChangePassword(ChangePasswordDto changePasswordDto);
         Task<Result<ResetPasswordEmailDto>> ResetPasswordSendEmail(ResetPasswordEmailDto resetPasswordEmailDto);
        Task<Result<string>> ResetPassword(ResetPasswordHandleDto resetPasswordHandleDto);
        Task<Result<string>> Delete(string id);
        Task<Result<string>> GetUserName();
        Task<Result<UserGetDto>> Profile();
        Task<Result<string>> SendVerificationCode(string email);
        Task<Result<string>> VerifyCode(VerifyCodeDto verifyCodeDto);
    }
}
