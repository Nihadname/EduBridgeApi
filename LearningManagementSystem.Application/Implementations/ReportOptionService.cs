using AutoMapper;
using CloudinaryDotNet.Actions;
using LearningManagementSystem.Application.Dtos.Auth;
using LearningManagementSystem.Application.Dtos.ReportOption;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Result<string>> DeleteFromUi(Guid id)
        {
            var reportOptionResult = await GetReportOption(id);
            if (!reportOptionResult.IsSuccess) return Result<string>.Failure(reportOptionResult.ErrorKey, reportOptionResult.Message, (ErrorType)reportOptionResult.ErrorType);
            reportOptionResult.Data.IsDeleted = true;
            await _unitOfWork.ReportOptionRepository.Delete(reportOptionResult.Data);
            await _unitOfWork.Commit();
            return Result<string>.Success("deleted from ui");
        }
        public async Task<Result<string>> Delete(Guid id)
        {
            var reportOptionResult=await GetReportOption(id);
            if (!reportOptionResult.IsSuccess) return Result<string>.Failure(reportOptionResult.ErrorKey, reportOptionResult.Message, (ErrorType)reportOptionResult.ErrorType);
            await _unitOfWork.ReportOptionRepository.Delete(reportOptionResult.Data);
            await _unitOfWork.Commit();
            return Result<string>.Success("deleted");
        }
        private async Task<Result<ReportOption>> GetReportOption(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Result<ReportOption>.Failure(null, "Invalid GUID provided.", ErrorType.ValidationError);
            }
            var existedReportOption = await _unitOfWork.ReportOptionRepository.GetEntity(s => s.Id == id && s.IsDeleted == false);
            if (existedReportOption == null)
            {
                return Result<ReportOption>.Failure("ReportedUserId", "Reported user not found", ErrorType.NotFoundError);
            }
            return Result<ReportOption>.Success(existedReportOption);

        }
    }
}
