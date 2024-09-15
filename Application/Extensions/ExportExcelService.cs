using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IUnitOfWork;
using Application.Models.DocumentModels;
using Application.Models.HelperModels;
using AutoMapper;
using ClosedXML.Excel;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Application.Extensions
{
    public class ExportExcelService : IExportExcelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExportExcelService(IUnitOfWork unitOfWork, IMapper mapper)
            => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<Response<string>> ImportDocument(IFormFile formFile)
        {
            if (formFile is not object || formFile.Length <= 0) return new Response<string>("File import is empty!", 404);

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)) return new Response<string>("Not Support file extension!", 409);

            var documents = await _unitOfWork.DocumentRepository.GetAllAsync();
            var documentList = _mapper.Map<List<ExcelDocumentModel>>(documents.Items);

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 4; row <= rowCount; row++)
                    {
                        documentList.Add(new ExcelDocumentModel
                        {
                            Title = worksheet?.Cells[row, 1]?.Value.ToString()?.Trim() ?? string.Empty,
                            Content = worksheet?.Cells[row, 2]?.Value.ToString()?.Trim() ?? string.Empty,
                            Description = worksheet?.Cells[row, 3]?.Value.ToString()?.Trim() ?? string.Empty,
                            DocumentStatus = worksheet?.Cells[row, 4]?.Value.ToString()?.Trim() ?? string.Empty,
                            Priority = worksheet?.Cells[row, 5]?.Value.ToString()?.Trim() ?? string.Empty
                        });
                    }
                }
            }
            var a = _mapper.Map<List<Document>>(documentList);
            _unitOfWork.DocumentRepository.AddRangeAsync(a);
            var isSuccess = await _unitOfWork.SaveChangesAsync();
            return isSuccess > 0
                ? new Response<string>("Import file excel success!")
                : new Response<string>("Import file excel fail!", 500);
        }
        public async Task<Response<byte[]>> SaveFileAsync()
        {
            var documents = await _unitOfWork.DocumentRepository.GetAllAsync();
            var documentList = _mapper.Map<List<ExcelDocumentModel>>(documents.Items);
            if (documentList is null || documentList.Count is 0)
            {
                return new Response<byte[]>("No document found", 404);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Documents");

            // Add the headers to the worksheet
            var headers = new[] { "Title", "Content", "Description", "DocumentStatus", "Priority", "Quantity", "Price", "CreatedBy" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }

            for (var i = 0; i < documentList.Count; i++)
            {
                var model = documentList[i];
                worksheet.Cell(i + 2, 1).Value = model.Title;
                worksheet.Cell(i + 2, 2).Value = model.Content;
                worksheet.Cell(i + 2, 3).Value = model.Description;
                worksheet.Cell(i + 2, 4).Value = model.DocumentStatus;
                worksheet.Cell(i + 2, 5).Value = model.Priority;
                worksheet.Cell(i + 2, 6).Value = model.Quantity;
                worksheet.Cell(i + 2, 7).Value = model.Price;
                worksheet.Cell(i + 2, 8).Value = model.CreatedBy;
            }

            // Convert the workbook to a byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return new Response<byte[]>(stream.ToArray());
        }
        public string CreateFileName()
        {
            var getCurrentDate = DateTime.UtcNow.ToString("yyyy_MM_dd");
            var fileName = $"Document{getCurrentDate}.xlsx";
            return fileName;
        }
        public string FormatSection() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
