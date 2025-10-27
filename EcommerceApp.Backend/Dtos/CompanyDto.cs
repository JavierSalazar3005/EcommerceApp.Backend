namespace EcommerceApp.Backend.Dtos
{
    public class CompanyDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public long UserId { get; set; }
    }
}