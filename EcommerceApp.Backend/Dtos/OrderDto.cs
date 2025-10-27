namespace EcommerceApp.Backend.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }
}