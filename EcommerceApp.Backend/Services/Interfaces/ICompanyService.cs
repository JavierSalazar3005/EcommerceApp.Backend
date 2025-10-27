using EcommerceApp.Backend.Dtos;

namespace EcommerceApp.Backend.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllAsync();
        Task<CompanyDto?> GetByIdAsync(long id);
        Task<CompanyDto> CreateAsync(CompanyDto dto);
        Task<CompanyDto> UpdateAsync(long id, CompanyDto dto);
        Task<bool> DeleteAsync(long id);
    }
}