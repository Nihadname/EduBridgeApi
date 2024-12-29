using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Address
{
    public class AddressReturnDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public AppUserInAdress appUserInAdress { get; set; }
    }
    
    public class AppUserInAdress{
      public string UserName { get; set; }
    }
}
