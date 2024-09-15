using Application.Models.OrderModels;

namespace Application.Models.DocumentModels
{
    public class DocumentModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Content { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string DocumentStatus { get; set; } = null!;
        public string Images { get; set; } = null!;
        public decimal TotalPrice => OrderDetails!.Sum(x => x.ItemPrice);
        public ICollection<OrderDetails>? OrderDetails { get; set; }
    }

    public class CreateDocumentModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Content { get; set; } = null!;
    }

    public class UpdateDocumentModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Content { get; set; } = null!;
    }
    public class ExcelDocumentModel
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Content { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string DocumentStatus { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
