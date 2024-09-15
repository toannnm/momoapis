using Domain.Enums;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public OrderStatusEnum OrderStatus { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentUrl { get; set; }

        public User User { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
