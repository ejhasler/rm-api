using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RestaurantManagerAPI.Controllers
{
    /// <summary>
    /// Controller for managing products in the restaurant stock.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            var productReadDtos = products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                PortionCount = p.PortionCount,
                Unit = p.Unit,
                PortionSize = p.PortionSize
            });

            return Ok(productReadDtos);
        }

        /// <summary>
        /// Gets a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productReadDto = new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                PortionCount = product.PortionCount,
                Unit = product.Unit,
                PortionSize = product.PortionSize
            };

            return Ok(productReadDto);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productCreateDto">The product data transfer object (DTO) containing the new product details.</param>
        /// <returns>The added product.</returns>
        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> AddProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = productCreateDto.Name,
                PortionCount = productCreateDto.PortionCount,
                Unit = productCreateDto.Unit,
                PortionSize = productCreateDto.PortionSize
            };

            var newProduct = await _productService.AddProductAsync(product);

            var productReadDto = new ProductReadDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                PortionCount = newProduct.PortionCount,
                Unit = newProduct.Unit,
                PortionSize = newProduct.PortionSize
            };

            return CreatedAtAction(nameof(GetProduct), new { id = productReadDto.Id }, productReadDto);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productUpdateDto">The updated product data transfer object (DTO).</param>
        /// <returns>A status indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReadDto>> UpdateProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            if (id != productUpdateDto.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = productUpdateDto.Name;
            existingProduct.PortionCount = productUpdateDto.PortionCount;
            existingProduct.Unit = productUpdateDto.Unit;
            existingProduct.PortionSize = productUpdateDto.PortionSize;

            await _productService.UpdateProductAsync(existingProduct);

            // Create a DTO to return the updated product data
            var updatedProductReadDto = new ProductReadDto
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                PortionCount = existingProduct.PortionCount,
                Unit = existingProduct.Unit,
                PortionSize = existingProduct.PortionSize
            };

            // Return the updated product data
            return Ok(updatedProductReadDto);
        }


        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A status indicating the result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }
}
