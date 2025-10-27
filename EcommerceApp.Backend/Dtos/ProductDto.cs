namespace EcommerceApp.Backend.Dtos
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

        public long CompanyId { get; set; }
        public string? CompanyName { get; set; } // opcional, útil en el catálogo
    }
}