namespace Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid DocumentId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public Order Order { get; set; }
        public Document Document { get; set; }
    }
}
