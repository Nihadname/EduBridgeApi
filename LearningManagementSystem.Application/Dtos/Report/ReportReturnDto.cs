using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Report
{
    public record ReportReturnDto
    {
        public string Description { get; init; }
        public UserReportReturnDto userReportReturnDto { get; init; }
        public ReportOptionInReportReturnDto optionInReportReturnDto { get; init; }
    }
}