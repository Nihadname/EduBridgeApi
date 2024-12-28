using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface IPhotoOrVideoService
    {
        Task<string> UploadPhotoAsync(IFormFile file);
        Task DeletePhotoAsync(string imageUrl);
    }
}
