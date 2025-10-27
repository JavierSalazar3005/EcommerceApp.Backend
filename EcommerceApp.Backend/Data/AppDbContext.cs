using Microsoft.EntityFrameworkCore;
using EcommerceApp.Backend.Models;

namespace EcommerceApp.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 🔹 DbSets = tablas que EF Core creará en la base de datos
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔸 Relación 1–1 entre User y Company
            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithOne(c => c.User)
                .HasForeignKey<Company>(c => c.UserId);

            // 🔸 Relación 1–N entre Company y Products
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔸 Relación 1–N entre User (Customer) y Orders
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔸 Relación 1–N entre Order y OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔸 Relación N–1 entre Product y OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔸 Relación N–1 entre Company y OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Company)
                .WithMany()
                .HasForeignKey(oi => oi.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔸 Restricciones y formatos
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(10, 2);
        }
    }
}
