namespace EcommerceApp.Backend.Dtos
{
    public class OrderItemDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public long CompanyId { get; set; }
    }
}