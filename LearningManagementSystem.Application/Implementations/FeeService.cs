using AutoMapper;
using CloudinaryDotNet;
using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LearningManagementSystem.Application.Implementations
{
    public class FeeService :  IFeeService
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
            if(feeCreateDto.DiscountPercentage is not null && feeCreateDto.DiscountPercentage > 0)
            {
                feeCreateDto.DiscountedPrice= feeCreateDto.Amount - (feeCreateDto.Amount * feeCreateDto.DiscountPercentage.Value) / 100;
            }
            var mappedFee= _mapper.Map<Fee>(feeCreateDto);
            await _unitOfWork.FeeRepository.Create(mappedFee);
            await _unitOfWork.Commit();
            return Result<string>.Success("fee created for student named");
        }
       public async Task CreateFeeToAllStudents(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var allStudents = await _unitOfWork.StudentRepository.GetAll(s => s.IsEnrolled == true && !s.IsDeleted, true, includes: new Func<IQueryable<Student>, IQueryable<Student>>[]
            {
                query => query.Include(s=>s.courseStudents).ThenInclude(s=>s.Course)
            });

                foreach (var student in allStudents)
                {
                    decimal amount = student.courseStudents.Where(x => x.Course != null).Select(x => x.Course.Price).Sum();
                    FeeCreateDto feeCreateDto = new()
                    {
                        Amount = amount,
                        StudentId = student.Id
                    };
                    await CreateFeeAndAssignToStudent(feeCreateDto);
                }
            }
        }
    }
}
