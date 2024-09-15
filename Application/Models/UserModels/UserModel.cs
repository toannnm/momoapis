using System.ComponentModel.DataAnnotations;

namespace Application.Models.UserModels
{
    public class RegisterModel
    {
        [Required, MinLength(3), RegularExpression(@"^[a-zA-Z0-9]+$"), DataType(DataType.Text)]
        public string Username { get; set; }
        [Required, Length(5, 20), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Text), MinLength(3)]
        public string FullName { get; set; }
        [Required, RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"), Length(0, 100), DataType(DataType.EmailAddress), EmailAddress()]
        public string Email { get; set; }
        [Required, StringLength(10), RegularExpression(@"^(84|0[3|5|7|8|9])+([0-9]{8})$"), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [MinLength(5), DataType(DataType.Text)]
        public string? Address { get; set; }
    }
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Images { get; set; }
        public string Role { get; set; }
        public DateTime? CreationDate { get; set; }
        public ICollection<OrderModels> Orders { get; set; }
    }
    public class OrderModels
    {
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentUrl { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
    }
}
