using EcommerceApp.Backend.Data;
using EcommerceApp.Backend.Dtos;
using EcommerceApp.Backend.Models;
using EcommerceApp.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Backend.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(string? search = null, long? companyId = null)
        {
            var query = _context.Products
                .Include(p => p.Company)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (companyId.HasValue)
                query = query.Where(p => p.CompanyId == companyId.Value);

            return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CompanyId = p.CompanyId,
                CompanyName = p.Company.Name
            }).ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetByCompanyAsync(long companyId)
        {
            return await _context.Products
                .Where(p => p.CompanyId == companyId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CompanyId = p.CompanyId
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(long id)
        {
            var p = await _context.Products.Include(p => p.Company).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return null;

            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CompanyId = p.CompanyId,
                CompanyName = p.Company.Name
            };
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var entity = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CompanyId = dto.CompanyId,
                IsActive = true
            };

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<ProductDto> UpdateAsync(long id, ProductDto dto)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) throw new Exception("Product not found.");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.Stock = dto.Stock;
            entity.ImageUrl = dto.ImageUrl;
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) return false;

            entity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
