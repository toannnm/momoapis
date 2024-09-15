using Domain.Enums;

namespace Domain.Entities
{
    public class Document : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Content { get; set; } = null!;
        public PriorityEnum Priority { get; set; }
        public DocumentStatusEnum DocumentStatus { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string>? Images { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
