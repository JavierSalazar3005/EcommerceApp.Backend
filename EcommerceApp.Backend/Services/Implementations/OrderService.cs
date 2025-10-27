using EcommerceApp.Backend.Data;
using EcommerceApp.Backend.Dtos;
using EcommerceApp.Backend.Models;
using EcommerceApp.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Backend.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CreateAsync(OrderDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                Total = dto.Total,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in dto.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null) throw new Exception($"Product {item.ProductId} not found.");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    CompanyId = product.CompanyId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            dto.Id = order.Id;
            dto.Status = order.Status;
            dto.CreatedAt = order.CreatedAt;
            return dto;
        }

        public async Task<OrderDto?> GetByIdAsync(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Total = order.Total,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    CompanyId = i.CompanyId
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    Total = o.Total,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> ConfirmAsync(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;
            if (order.Status != "Pending") throw new Exception("Order already processed.");

            foreach (var item in order.Items)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new Exception($"Insufficient stock for {item.Product.Name}.");
                item.Product.Stock -= item.Quantity;
            }

            order.Status = "Confirmed";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;

            if (order.Status == "Confirmed")
            {
                foreach (var item in order.Items)
                    item.Product.Stock += item.Quantity; // reponer stock
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
