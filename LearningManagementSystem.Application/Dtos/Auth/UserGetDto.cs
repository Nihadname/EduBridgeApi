﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Auth
{
    public record UserGetDto
    {
        public string FullName { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsBlocked { get; set; }
    }
}
