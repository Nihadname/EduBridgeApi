﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Auth
{
    public class UserUpdateImageDto
    {
        public IFormFile Image {get; set;}
    }
}
