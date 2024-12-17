using LearningManagementSystem.Application.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Ai
{
    public class UserData
    {
        public int Age { get; set; }           
        public string StudyTime { get; set; }  
        public string Absences { get; set; }   
        public string Failures { get; set; }
    }
}
