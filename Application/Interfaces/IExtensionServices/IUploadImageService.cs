using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IExtensionServices
{
    public interface IUploadImageService
    {
        Task<List<string>> Upload(ICollection<IFormFile> files);
    }
}