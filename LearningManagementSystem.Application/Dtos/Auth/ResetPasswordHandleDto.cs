﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Auth
{
    public class ResetPasswordHandleDto
    {
        public ResetPasswordTokenAndEmailDto ResetPasswordTokenAndEmailDto { get; set; }
        public ResetPasswordDto resetPasswordDto    { get; set; }
    }
}
