﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Auth
{
    public record AuthResponseDto
    {
        public bool IsSuccess { get; init; }
        public string Token { get; init; }
    }
}
