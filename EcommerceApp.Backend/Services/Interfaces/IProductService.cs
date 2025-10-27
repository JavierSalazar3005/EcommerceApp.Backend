using EcommerceApp.Backend.Dtos;

namespace EcommerceApp.Backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync(string? search = null, long? companyId = null);
        Task<IEnumerable<ProductDto>> GetByCompanyAsync(long companyId);
        Task<ProductDto?> GetByIdAsync(long id);
        Task<ProductDto> CreateAsync(ProductDto dto);
        Task<ProductDto> UpdateAsync(long id, ProductDto dto);
        Task<bool> DeleteAsync(long id);
    }
}