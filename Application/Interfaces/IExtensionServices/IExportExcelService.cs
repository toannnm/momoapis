using Application.Models.HelperModels;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IExtensionServices
{
    public interface IExportExcelService
    {
        string CreateFileName();
        string FormatSection();
        Task<Response<string>> ImportDocument(IFormFile formFile);
        Task<Response<byte[]>> SaveFileAsync();
    }
}