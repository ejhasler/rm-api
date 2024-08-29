using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Controllers;

/// <summary>
/// Represents a controller for managing products in the restaurant stock.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>29.08.2021</date>
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
    /// Gets all products from the database.
    /// </summary>
    /// <returns>All the products in the database</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product with the specified unique identifier.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
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
        return Ok(product);
    }

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
    {
        // Check model validation
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newProduct = await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (id != product.Id)
        {
            return BadRequest("Product ID mismatch.");
        }

        // Check model validation
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _productService.UpdateProductAsync(product);
        return NoContent();
    }

    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid product ID.");
        }

        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}
