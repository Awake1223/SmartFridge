using FridgeRecipe.Domain.Models;
using FridgeRecipe.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FridgeRecipe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _product;

        public ProductController(IProductRepository product)
        {
            _product = product;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProduct()
        {
            var products = await _product.GetAllAsync();
            return Ok (products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(Guid id) 
        {
            var product = await _product.GetByIdAsync(id);

            if(product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel product)
        {
            product.AddedDate = DateTime.UtcNow;

            await _product.AddAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductModel product) 
        {
            if(id != product.Id)
                return BadRequest();

            await _product.UpdateAsync(product);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _product.DeleteAsync(id);

            return NoContent();
        }
    }
}
