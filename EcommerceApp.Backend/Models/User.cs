using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Backend.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Role { get; set; } = "Customer"; // Admin, Company, Customer

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔹 Relación 1–1 (solo si este user es una empresa)
        public Company? Company { get; set; }

        // 🔹 Relación 1–N: un cliente puede tener muchos pedidos
        public ICollection<Order>? Orders { get; set; }
    }
}