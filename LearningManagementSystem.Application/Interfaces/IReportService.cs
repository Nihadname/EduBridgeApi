﻿using LearningManagementSystem.Application.Dtos.Report;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IReportService
    {
        Task<ReportReturnDto> Create(ReportCreateDto reportCreateDto);
        Task<string> VerifyReport(Guid id);
        Task<Result<string>> DeleteForUser(Guid id);
        Task<Result<string>> DeleteForAdmin(Guid id);
    }
}
