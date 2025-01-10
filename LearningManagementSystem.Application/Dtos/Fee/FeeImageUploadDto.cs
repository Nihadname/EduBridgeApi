using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Fee
{
    public record FeeImageUploadDto
    {
        public  Guid Id { get; set; }   
        public IFormFile image {  get; init; }
    }
}
