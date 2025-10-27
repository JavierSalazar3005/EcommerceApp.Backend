using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Backend.Models
{
    public class Product
    {
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [MaxLength(250)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        // 🔹 Relación N–1 con Company
        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }

        // 🔹 Relación N–N implícita vía OrderItem
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}