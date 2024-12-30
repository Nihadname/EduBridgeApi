using LearningManagementSystem.Application.Dtos.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IAddressService
    {
        Task<AddressReturnDto> Create(AddressCreateDto addressCreateDto);
        Task<string> DeleteForUser(Guid id);
    }
}
