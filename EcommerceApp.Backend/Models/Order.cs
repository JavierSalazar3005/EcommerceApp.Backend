using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Backend.Models
{
    public class Order
    {
        public long Id { get; set; }

        // 🔹 Relación N–1 con User (el cliente que hace el pedido)
        [ForeignKey("Customer")]
        public long CustomerId { get; set; }
        public User Customer { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔹 Relación 1–N con OrderItem
        public ICollection<OrderItem>? Items { get; set; }
    }
}