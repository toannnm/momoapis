using Application.Extensions;
using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IServices;
using Application.Interfaces.IUnitOfWork;
using Application.Models.DocumentModels;
using Application.Models.HelperModels;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Document = Domain.Entities.Document;

namespace Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;
        private readonly IUploadImageService _imageService;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claimService, IUploadImageService imageService)
            => (_unitOfWork, _mapper, _claimService, _imageService) = (unitOfWork, mapper, claimService, imageService);

        public async Task<Response<Pagination<DocumentModel>>> GetDocumentsAsync(int pageIndex = 1, int pageSize = 10)
        {
            var data = await _unitOfWork.DocumentRepository.GetAllAsync(pageIndex, pageSize, x => x.Include(x => x.OrderDetails)!);

            if (data is null || data.Items.Count is 0)
                return new Response<Pagination<DocumentModel>>("List is empty!", 404);

            var result = _mapper.Map<Pagination<DocumentModel>>(data);
            return new Response<Pagination<DocumentModel>>(result);
        }

        public async Task<Response<DocumentModel>> GetDocumentByIdAsync(Guid id)
        {
            var data = await _unitOfWork.DocumentRepository.GetByIdAsync(id);

            if (data is null)
                return new Response<DocumentModel>("Document not found!", 404);

            var result = _mapper.Map<DocumentModel>(data);
            return new Response<DocumentModel>(result);
        }

        public async Task<Response<DocumentModel>> AddDocumentAsync(CreateDocumentModel model, List<IFormFile> formFiles)
        {
            var currentUserId = _claimService.GetCurrentUserId;
            if (currentUserId is null || currentUserId == Guid.Empty)
                return new Response<DocumentModel>("Not login yet", 401);

            var document = _mapper.Map<Document>(model);

            var listImages = await _imageService.Upload(formFiles);
            document.Images ??= [];
            document.Images.AddRange(listImages);
            await _unitOfWork.DocumentRepository.AddEntityAsync(document);

            var isSuccess = await _unitOfWork.SaveChangesAsync();

            return isSuccess > 0
                ? new Response<DocumentModel>(_mapper.Map<DocumentModel>(document))
                : new Response<DocumentModel>("Add document failed!", 400);
        }

        public async Task<Response<DocumentModel>> Update(Guid id, UpdateDocumentModel model, List<IFormFile> formFiles)
        {
            var currentUserId = _claimService.GetCurrentUserId;
            if (currentUserId is null || currentUserId == Guid.Empty)
                return new Response<DocumentModel>("Not login yet", 401);

            var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);

            if (document is null)
                return new Response<DocumentModel>("Document not found!", 404);

            var listImages = await _imageService.Upload(formFiles);
            document.Images ??= [];
            document.Images = listImages;

            _mapper.Map(model, document);

            _unitOfWork.DocumentRepository.Update(document);

            var isSuccess = await _unitOfWork.SaveChangesAsync();

            return isSuccess > 0
                ? new Response<DocumentModel>(_mapper.Map<DocumentModel>(document))
                : new Response<DocumentModel>("Update document failed!", 400);
        }

        public async Task<Response<DocumentModel>> Delete(Guid id)
        {
            var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);

            if (document is null)
                return new Response<DocumentModel>("Document not found!", 404);

            _unitOfWork.DocumentRepository.SoftRemove(document);

            var isSuccess = await _unitOfWork.SaveChangesAsync();

            return isSuccess > 0
                ? new Response<DocumentModel>(_mapper.Map<DocumentModel>(document))
                : new Response<DocumentModel>("Delete document failed!", 400);
        }

        public async Task<Response<Document>> UploadImagesToCloudinary(Guid id, List<IFormFile> files)
        {
            var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);

            if (document is null) return new Response<Document>("Not found", 404);

            var images = await _imageService.Upload(files);
            document.Images ??= [];
            document.Images.AddRange(images);

            _unitOfWork.DocumentRepository.Update(document);
            var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;
            return isSuccess ? new Response<Document>(document) : new Response<Document>("Upload Fail!", 400);
        }

        public async Task<Response<Pagination<DocumentModel>>> FilterDocumentAsync(string? query, int pageIndex = 1, int pageSize = 10)
        {
            var result = new Pagination<DocumentModel>();
            var data = new Pagination<Document>();

            var priorityEnum = Enum.GetValues(typeof(PriorityEnum))
                       .Cast<PriorityEnum>()
                       .Select(x => x.ToString().ToLower())
                       .ToList();

            var documentEnum = Enum.GetValues(typeof(DocumentStatusEnum))
                       .Cast<PriorityEnum>()
                       .Select(x => x.ToString().ToLower())
                       .ToList();
            var isDecimal = decimal.TryParse(query, out decimal asQueyDecimal);

            var isPriorityEnum = Enum.TryParse(typeof(PriorityEnum), query, true, out var priorityValue);

            var isDocumentEnum = Enum.TryParse(typeof(DocumentStatusEnum), query, true, out var documentValue);

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.ToLower();
                data = await _unitOfWork.DocumentRepository
                    .Filter(
                    x => (query != null && x.Title.ToLower().Contains(query))
                    || (query != null && x.Description!.ToLower().Contains(query))
                    || (query != null && x.Content.ToLower().Contains(query))
                    || (isPriorityEnum && (priorityEnum.Contains(query!) && x.Priority == (PriorityEnum)Enum.Parse(typeof(PriorityEnum), query!)))
                    || (isDocumentEnum && (documentEnum.Contains(query!) && x.DocumentStatus == (DocumentStatusEnum)Enum.Parse(typeof(DocumentStatusEnum), query!)))
                    || (isDecimal && (x.Quantity == asQueyDecimal))
                    || (isDecimal && (x.Price == asQueyDecimal))
                        , x => x.Include(x => x.OrderDetails),
                        pageIndex,
                        pageSize,
                        x => x.CreationDate!,
                        SortDirectionEnum.Descending);
                result = _mapper.Map<Pagination<DocumentModel>>(data);
            }
            else
            {
                data = await _unitOfWork.DocumentRepository
                    .Filter(null, x => x.Include(x => x.OrderDetails),
                        pageIndex,
                        pageSize,
                        x => x.CreationDate!,
                        SortDirectionEnum.Descending);
                result = _mapper.Map<Pagination<DocumentModel>>(data);
            }

            return new Response<Pagination<DocumentModel>>(result);
        }

        public async Task<Response<Pagination<DocumentModel>>> FilterDocumentAsync(FilterDocumentModel model, int pageIndex = 0, int pageSize = 10)
        {
            var result = new Pagination<DocumentModel>();
            var data = new Pagination<Document>();

            if (!string.IsNullOrWhiteSpace(model.Content) || !string.IsNullOrWhiteSpace(model.Description) || !string.IsNullOrWhiteSpace(model.Title) || model.DocumentStatus.HasValue || model.Priority.HasValue)
            {
                data = await _unitOfWork.DocumentRepository.Filter(
                    x => (model.Content != null && x.Content.ToLower().Contains(model.Content.ToLower()))
                    || (model.Description != null && x.Description!.ToLower().Contains(model.Description.ToLower()))
                    || (model.Title != null && x.Title.ToLower().Contains(model.Title.ToLower()))
                    || x.DocumentStatus == model.DocumentStatus
                    || x.Priority == model.Priority, null,
                    pageIndex, pageSize, x => x.CreationDate!, SortDirectionEnum.Descending);
                result = _mapper.Map<Pagination<DocumentModel>>(data);
            }
            else
            {
                data = await _unitOfWork.DocumentRepository
                    .Filter(null, x => x.Include(x => x.OrderDetails),
                        pageIndex,
                        pageSize,
                        x => x.CreationDate!,
                        SortDirectionEnum.Descending);
                result = _mapper.Map<Pagination<DocumentModel>>(data);
            }

            return new Response<Pagination<DocumentModel>>(result);
        }
    }
}
