using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Address
{
    public record AddressListItemDto
    {
        public string Country { get; init; }
        public string City { get; init; }
        public string Region { get; init; }
        public string Street { get; set; }
        public AppUserInAdress appUserInAdress { get; init; }
    }
}
