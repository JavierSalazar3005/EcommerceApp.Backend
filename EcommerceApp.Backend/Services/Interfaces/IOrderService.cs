using EcommerceApp.Backend.Dtos;

namespace EcommerceApp.Backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(OrderDto dto);
        Task<OrderDto?> GetByIdAsync(long id);
        Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId);
        Task<bool> ConfirmAsync(long id);
        Task<bool> CancelAsync(long id);
    }
}