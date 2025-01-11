using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IFeeService
    {
        Task<Result<string>> CreateFeeAndAssignToStudent(FeeCreateDto feeCreateDto);
        Task CreateFeeToAllStudents(CancellationToken stoppingToken);
        Task<Result<string>> UploadImageOfBankTransfer(FeeImageUploadDto feeImageUploadDto);
        Task<Result<string>> VerifyFee(Guid id);
    }
}
