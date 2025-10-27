using EcommerceApp.Backend.Data;
using EcommerceApp.Backend.Dtos;
using EcommerceApp.Backend.Models;
using EcommerceApp.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Backend.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            return await _context.Companies
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TaxId = c.TaxId,
                    Address = c.Address,
                    Phone = c.Phone,
                    UserId = c.UserId
                })
                .ToListAsync();
        }

        public async Task<CompanyDto?> GetByIdAsync(long id)
        {
            var c = await _context.Companies.FindAsync(id);
            if (c == null) return null;

            return new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                TaxId = c.TaxId,
                Address = c.Address,
                Phone = c.Phone,
                UserId = c.UserId
            };
        }

        public async Task<CompanyDto> CreateAsync(CompanyDto dto)
        {
            var entity = new Company
            {
                Name = dto.Name,
                TaxId = dto.TaxId,
                Address = dto.Address,
                Phone = dto.Phone,
                UserId = dto.UserId
            };

            _context.Companies.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<CompanyDto> UpdateAsync(long id, CompanyDto dto)
        {
            var entity = await _context.Companies.FindAsync(id);
            if (entity == null) throw new Exception("Company not found.");

            entity.Name = dto.Name;
            entity.TaxId = dto.TaxId;
            entity.Address = dto.Address;
            entity.Phone = dto.Phone;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Companies.FindAsync(id);
            if (entity == null) return false;

            _context.Companies.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
