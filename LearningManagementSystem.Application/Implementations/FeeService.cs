using AutoMapper;
using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class FeeService : BackgroundService, IFeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<string>> CreateFeeAndAssignToStudent(FeeCreateDto feeCreateDto)
        {
            if (!await _unitOfWork.StudentRepository.isExists(s => s.Id == feeCreateDto.StudentId)) 
                return Result<string>.Failure("StudentId", "Id is invalid", ErrorType.NotFoundError);
            var lastFee = await _unitOfWork.FeeRepository.GetLaastFeeAsync(f => f.StudentId == feeCreateDto.StudentId && !f.IsDeleted);
            feeCreateDto.DueDate = lastFee != null ? lastFee.DueDate.AddMonths(1) : DateTime.Now.AddMonths(1);
            feeCreateDto.PaymentStatus=PaymentStatus.Pending;
            if(feeCreateDto.DiscountPercentage is not null || feeCreateDto.DiscountPercentage.Value > 0)
            {
                feeCreateDto.DiscountedPrice= feeCreateDto.Amount - (feeCreateDto.Amount * feeCreateDto.DiscountPercentage.Value) / 100;
            }
            var mappedFee= _mapper.Map<Fee>(feeCreateDto);
            await _unitOfWork.FeeRepository.Create(mappedFee);
            await _unitOfWork.Commit();
            return Result<string>.Success("fee created for student named");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Now.Day is not 1) throw new CustomException(400, "Day", "it is not first day ");
                var allEnroledStudents=await _unitOfWork.StudentRepository.GetAll(s=>s.IsEnrolled==true&&!s.IsDeleted,true);


            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
