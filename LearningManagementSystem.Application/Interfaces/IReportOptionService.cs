using LearningManagementSystem.Application.Dtos.ReportOption;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IReportOptionService
    {
        Task<ReportOptionReturnDto> Create(ReportOptionCreateDto reportOptionCreateDto);
    }
}
