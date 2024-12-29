using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class PhotoOrVideoService : IPhotoOrVideoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoOrVideoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> DeleteMediaAsync(string mediaUrl, ResourceType resourceType)
        {
            string publicId = await ExtractPublicIdFromUrl(mediaUrl);
            var deletionParams = new DeletionParams(publicId) { ResourceType = resourceType };
            var result = await _cloudinary.DestroyAsync(deletionParams);
            if (result.Result != "ok")
                throw new CustomException(500, "Image", "Image delete error");
            return "deleted";
        }
        public async Task<string> UploadMediaAsync(IFormFile file, bool isVideo = false)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            object uploadResult;
            if (isVideo)
            {
                uploadResult = new VideoUploadResult();
            }
            else
            {
                uploadResult = new ImageUploadResult();
            }

            using (var stream = file.OpenReadStream())
            {
                if (isVideo)
                {
                    var uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill") // Adjust for videos
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                else
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }


            }
            if (isVideo)
            {
                var result = (VideoUploadResult)uploadResult;
                if (result.Error != null)
                {
                    throw new CustomException(400, result.Error.Message);
                }
                return result.SecureUrl.ToString();
            }
            else
            {
                var result = (ImageUploadResult)uploadResult;
                if (result.Error != null)
                {
                    throw new CustomException(400,result.Error.Message);
                }
                return result.SecureUrl.ToString();
            }
        }
    
        private async Task<string> ExtractPublicIdFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string[] segments = uri.Segments;

            string lastSegment = segments[^1];
            string publicId = lastSegment.Substring(0, lastSegment.LastIndexOf('.'));

            return publicId;
        }
    }
}
