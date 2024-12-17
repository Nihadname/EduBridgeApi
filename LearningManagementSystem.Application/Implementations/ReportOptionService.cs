using AutoMapper;
using LearningManagementSystem.Application.Dtos.ReportOption;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
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
        public async Task<ReportOptionReturnDto> Create(ReportOptionCreateDto reportOptionCreateDto)
        {
            if(await _unitOfWork.ReportOptionRepository.isExists(s => s.IsDeleted == false && s.Name.ToLower() == reportOptionCreateDto.Name.ToLower()))
            {
                throw new CustomException(400, "This name already exists");
            }
            var mappedReportOption = _mapper.Map<ReportOption>(reportOptionCreateDto);
            await _unitOfWork.ReportOptionRepository.Create(mappedReportOption);
            await _unitOfWork.Commit();
            var responseReportOption=_mapper.Map<ReportOptionReturnDto>(mappedReportOption);
            return responseReportOption;
        }
    }
}
