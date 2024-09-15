using Application.Interfaces.IExtensionServices;
using Application.Models.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Extensions
{
    public class UploadImageService : IUploadImageService
    {

        private readonly CloudinarySection _cloudinarySection;
        private readonly Cloudinary _cloudinary;

        public UploadImageService(IOptions<CloudinarySection> cloudinary)
        {
            _cloudinarySection = cloudinary.Value;
            var account = new Account(_cloudinarySection.CloudName, _cloudinarySection.ApiKey, _cloudinarySection.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<List<string>> Upload(ICollection<IFormFile> files)
        {
            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, memoryStream),
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)
                {
                    throw new Exception($"Cloudinary error occurred: {result.Error.Message}");
                }

                imageUrls.Add(result.SecureUrl.ToString());
            }

            return imageUrls;
        }
    }
}
