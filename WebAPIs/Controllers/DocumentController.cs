using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IServices;
using Application.Models.DocumentModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIs.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IExportExcelService _exportExcelService;

        public DocumentController(IDocumentService documentService, IExportExcelService exportExcelService)
        => (_documentService, _exportExcelService) = (documentService, exportExcelService);

        [HttpGet]
        public async Task<ActionResult> GetDocuments(int pageIndex = 1, int pageSize = 10)
        {
            var result = await _documentService.GetDocumentsAsync(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDocumentById([FromRoute] Guid id)
        {
            var result = await _documentService.GetDocumentByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddDocument([FromForm] CreateDocumentModel model, List<IFormFile> formFiles)
        {
            var result = await _documentService.AddDocumentAsync(model, formFiles);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDocument([FromRoute] Guid id, [FromForm] UpdateDocumentModel model, List<IFormFile> formFiles)
        {
            var result = await _documentService.Update(id, model, formFiles);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> UploadImage(Guid id, List<IFormFile> files)
        {
            var result = await _documentService.UploadImagesToCloudinary(id, files);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> FilterDocuments(string? query, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _documentService.FilterDocumentAsync(query, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> FilterDocumentsFromQuery([FromQuery] FilterDocumentModel model, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _documentService.FilterDocumentAsync(model, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> ExportDocumentsToExcel()
        {
            var content = await _exportExcelService.SaveFileAsync();
            var fileName = _exportExcelService.CreateFileName();
            return File(content.Result!, _exportExcelService.FormatSection(), fileName);
        }

        [HttpPost]
        public async Task<ActionResult> ImportDocumentsToExcel(IFormFile formFile)
        {
            var result = await _exportExcelService.ImportDocument(formFile);
            return Ok(result);
        }
    }
}
