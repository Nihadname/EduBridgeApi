using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Entities
{
    public class AppUser:IdentityUser
    {
        public string fullName { get; set; }
        public string? Image { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get;  set; }
        public DateTime? BlockedUntil { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age => CalculateAgeOfUser(BirthDate);
        public Address Address { get; set; }
        private int  CalculateAgeOfUser(DateTime birthDate)
        {
            var Now=DateTime.Now;
            int age = Now.Year - birthDate.Year;

            if (Now.Month < birthDate.Month || (Now.Month == birthDate.Month && Now.Day < birthDate.Day))
                age--;

            return age;
        }
    }
}
