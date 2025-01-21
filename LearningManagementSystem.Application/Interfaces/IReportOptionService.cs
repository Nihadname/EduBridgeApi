using LearningManagementSystem.Application.Dtos.ReportOption;
using LearningManagementSystem.Core.Entities.Common;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IReportOptionService
    {
        Task<Result<ReportOptionReturnDto>> Create(ReportOptionCreateDto reportOptionCreateDto);
    }
}
