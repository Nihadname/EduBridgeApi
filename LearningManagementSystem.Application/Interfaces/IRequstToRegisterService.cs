using LearningManagementSystem.Application.Dtos.RequstToRegister;
using LearningManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IRequstToRegisterService
    {
        Task<string> Create(RequstToRegisterCreateDto requstToRegisterCreateDto);
        Task<string> SendAcceptanceEmail(Guid id);
        Task<string> VerifyExistenceOfEmailUser(string token);
        Task<IEnumerable<RequestToRegisterListItemDto>> GetAll();
            }
}
