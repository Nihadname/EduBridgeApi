using AutoMapper;
using LearningManagementSystem.Application.Dtos.ReportOption;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;

namespace LearningManagementSystem.Application.Implementations
{
    public class ReportOptionService: IReportOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper   _mapper;

        public ReportOptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<ReportOptionReturnDto>> Create(ReportOptionCreateDto reportOptionCreateDto)
        {
            if(await _unitOfWork.ReportOptionRepository.isExists(s => s.IsDeleted == false && s.Name.ToLower() == reportOptionCreateDto.Name.ToLower()))
            {
                return Result<ReportOptionReturnDto>.Failure(null, "This name already exists", ErrorType.BusinessLogicError);
            }
            var mappedReportOption = _mapper.Map<ReportOption>(reportOptionCreateDto);
            await _unitOfWork.ReportOptionRepository.Create(mappedReportOption);
            await _unitOfWork.Commit();
            var responseReportOption=_mapper.Map<ReportOptionReturnDto>(mappedReportOption);
            return Result< ReportOptionReturnDto>.Success(responseReportOption);
        }
    }
}
