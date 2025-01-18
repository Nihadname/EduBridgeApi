using LearningManagementSystem.Application.Dtos.Address;
using LearningManagementSystem.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IAddressService
    {
        Task<Result<AddressReturnDto>> Create(AddressCreateDto addressCreateDto);
        Task<Result<string>> DeleteForUser(Guid id);
        Task<Result<PaginationDto<AddressListItemDto>>> GetAll(int pageNumber = 1,
           int pageSize = 10, string searchQuery = null);
    }
}
