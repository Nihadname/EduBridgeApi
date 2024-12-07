using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Parent
{
    public record ParentRegisterDto
    {
        public RegisterDto Register { get; set; }
        public ParentCreateDto Parent { get; set; }
    }
}
