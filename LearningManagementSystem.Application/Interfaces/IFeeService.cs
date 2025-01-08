using LearningManagementSystem.Application.Dtos.Fee;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    internal interface IFeeService
    {
        Task<Result<string>> CreateFeeAndAssignToStudent(FeeCreateDto feeCreateDto);
        Task CreateFeeToAllStudents();
    }
}
