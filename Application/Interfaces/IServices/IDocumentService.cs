using Application.Extensions;
using Application.Models.DocumentModels;
using Application.Models.HelperModels;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices
{
    public interface IDocumentService
    {
        Task<Response<DocumentModel>> AddDocumentAsync(CreateDocumentModel model, List<IFormFile> formFiles);
        Task<Response<DocumentModel>> Delete(Guid id);
        Task<Response<Pagination<DocumentModel>>> GetDocumentsAsync(int pageIndex = 1, int pageSize = 10);
        Task<Response<DocumentModel>> GetDocumentByIdAsync(Guid id);
        Task<Response<DocumentModel>> Update(Guid id, UpdateDocumentModel model, List<IFormFile> formFiles);
        Task<Response<Document>> UploadImagesToCloudinary(Guid id, List<IFormFile> files);
        Task<Response<Pagination<DocumentModel>>> FilterDocumentAsync(string? query, int pageIndex = 1, int pageSize = 10);
        Task<Response<Pagination<DocumentModel>>> FilterDocumentAsync(FilterDocumentModel model, int pageNumber = 0, int pageSize = 10);
    }
}