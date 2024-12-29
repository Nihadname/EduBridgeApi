using CloudinaryDotNet.Actions;
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
        Task<string> UploadMediaAsync(IFormFile file, bool isVideo = false);
        Task<string> DeleteMediaAsync(string mediaUrl, ResourceType resourceType);
    }
}
