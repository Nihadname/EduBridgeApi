using LearningManagementSystem.Application.Dtos.ReportOption;
using LearningManagementSystem.Core.Entities.Common;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IReportOptionService
    {
        Task<Result<string>> DeleteFromUi(Guid id);
        Task<Result<string>> Delete(Guid id);
        Task<Result<ReportOptionReturnDto>> Create(ReportOptionCreateDto reportOptionCreateDto);
    }
}
