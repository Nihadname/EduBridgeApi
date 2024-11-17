using LearningManagementSystem.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Teacher
{
    public class TeacherRegistrationDto
    {
        public RegisterDto Register { get; set; }
        public TeacherCreateDto Teacher { get; set; }
    }
}
