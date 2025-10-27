using EcommerceApp.Backend.Dtos;
using EcommerceApp.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // 🔹 Crear pedido (desde carrito)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDto dto)
        {
            // Obtener el userId del token JWT
            var userId = long.Parse(User.FindFirstValue("userId")!);
            dto.CustomerId = userId;

            var created = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = long.Parse(User.FindFirstValue("userId")!);
            var orders = await _orderService.GetByCustomerAsync(userId);
            return Ok(orders);
        }

        [HttpPost("{id:long}/confirm")]
        public async Task<IActionResult> Confirm(long id)
        {
            await _orderService.ConfirmAsync(id);
            return Ok(new { message = "Order confirmed" });
        }

        [HttpPost("{id:long}/cancel")]
        public async Task<IActionResult> Cancel(long id)
        {
            await _orderService.CancelAsync(id);
            return Ok(new { message = "Order cancelled" });
        }
    }
}
