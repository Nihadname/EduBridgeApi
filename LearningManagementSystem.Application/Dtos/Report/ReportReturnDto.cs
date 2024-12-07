using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Report
{
    public record ReportReturnDto
    {
        public string Description { get; set; }
        public UserReportReturnDto userReportReturnDto { get; set; }
        public ReportOptionInReportReturnDto optionInReportReturnDto { get; set; }
    }
}