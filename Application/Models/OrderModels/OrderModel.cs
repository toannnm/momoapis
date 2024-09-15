using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Models.OrderModels
{
    public class OrderModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum OrderStatus { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }
    }
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public Guid DocumentId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
    public class CreateOrderModel
    {
        public Guid UserId { get; set; }
        public List<CreateOrderDetail>? OrderDetails { get; set; }
    }
    public class CreateOrderDetail
    {
        public Guid DocumentId { get; set; }
        public int Quantity { get; set; }
    }

}
