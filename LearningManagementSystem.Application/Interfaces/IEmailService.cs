﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(string from, string to, string subject, string body, string smtpHost, int smtpPort, bool enableSsl, string smtpUser, string smtpPass);
        public Task SendEmailAsyncToManyPeople(string from, List<string> recipients, string subject, string body, string smtpHost, int smtpPort, bool enableSsl, string smtpUser, string smtpPass);
    }
}
