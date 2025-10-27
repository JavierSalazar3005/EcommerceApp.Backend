using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Backend.Models
{
    public class Company
    {
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string TaxId { get; set; } = string.Empty; // NIT, CUIT, etc.

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔹 Relación 1–1 con User
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        // 🔹 Relación 1–N con Product
        public ICollection<Product>? Products { get; set; }
    }
}