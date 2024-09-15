using Domain.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Address { get; set; }
        public List<string>? Images { get; set; }
        public RoleEnum Role { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}

