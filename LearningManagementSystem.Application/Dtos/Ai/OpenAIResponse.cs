using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LearningManagementSystem.Application.Implementations.RequstToRegisterService;

namespace LearningManagementSystem.Application.Dtos.Ai
{
    public class OpenAIResponse
    {
        public Choice[] Choices { get; set; }

    }
}
