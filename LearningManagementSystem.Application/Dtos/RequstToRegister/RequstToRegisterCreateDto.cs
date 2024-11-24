using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.RequstToRegister
{
    public class RequstToRegisterCreateDto
    {
        public string FullName { get; set; }
        public int Age { get;  set; }
        public bool IsParent { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> ExistedCourses { get; set; }
        public string ChoosenCourse { get; set; }
        public string ChildName { get; set; }
        public int? ChildAge { get; set; }
        public string Email { get; set; }
    }
}
