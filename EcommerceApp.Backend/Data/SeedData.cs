using EcommerceApp.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Backend.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Aseguramos que la BD esté creada
            await context.Database.MigrateAsync();

            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<User>();

                // 🔹 Usuario administrador
                var admin = new User
                {
                    Email = "admin@ecommerce.com",
                    Role = "Admin"
                };
                admin.PasswordHash = hasher.HashPassword(admin, "Admin123*");

                context.Users.Add(admin);
                await context.SaveChangesAsync();

                // 🔹 Usuario empresa demo
                var companyUser = new User
                {
                    Email = "demo@store.com",
                    Role = "Company"
                };
                companyUser.PasswordHash = hasher.HashPassword(companyUser, "Store123*");
                context.Users.Add(companyUser);
                await context.SaveChangesAsync();

                // 🔹 Empresa asociada
                var company = new Company
                {
                    Name = "Demo Store",
                    TaxId = "123456789",
                    Address = "Calle Principal #45",
                    Phone = "70012345",
                    UserId = companyUser.Id
                };
                context.Companies.Add(company);
                await context.SaveChangesAsync();

                // 🔹 Producto ejemplo
                var product = new Product
                {
                    Name = "Camiseta Básica",
                    Description = "Camiseta de algodón color blanco",
                    Price = 49.99M,
                    Stock = 50,
                    CompanyId = company.Id
                };
                context.Products.Add(product);

                await context.SaveChangesAsync();
            }
        }
    }
}
