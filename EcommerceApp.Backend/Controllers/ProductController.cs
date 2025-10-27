using EcommerceApp.Backend.Dtos;
using EcommerceApp.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // 🔹 Catálogo público
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] long? companyId)
        {
            var products = await _productService.GetAllAsync(search, companyId);
            return Ok(products);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // 🔹 Panel empresa
        [HttpGet("company/{companyId:long}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> GetByCompany(long companyId)
        {
            return Ok(await _productService.GetByCompanyAsync(companyId));
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var created = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Update(long id, [FromBody] ProductDto dto)
        {
            return Ok(await _productService.UpdateAsync(id, dto));
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
