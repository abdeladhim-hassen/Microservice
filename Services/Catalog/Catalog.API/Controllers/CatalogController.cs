using Catalog.API.Models;
using Catalog.API.Repositories.ProductRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger) : ControllerBase
    {
        private readonly IProductRepository _repository = productRepository;
        private readonly ILogger<CatalogController> _logger = logger;
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _repository.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProductById(id);
            if (product == null)
            {
                _logger.LogError("Product with id: {ProductId} not found.", id);
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("products-by-category/{category}", Name = "GetProductByCategory")]
        public async Task<ActionResult<List<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpGet("products-by-name/{name}", Name = "GetProductByName")]
        public async Task<ActionResult<List<Product>>> GetProductByName(string name)
        {
            var products = await _repository.GetProductByName(name);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }
    }
}
