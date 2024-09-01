using RestaurantManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Services;

/// <summary>
/// Represents an interface for a product service.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>31.08.2024</date>
public interface IProductService
{
    /// <summary>
    /// Gets all products from the database.
    /// </summary>
    /// <returns
    Task<IEnumerable<Product>> GetAllProductsAsync();

    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product with the specified unique identifier.</returns>
    Task<Product> GetProductByIdAsync(int id);

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    Task<Product> AddProductAsync(Product product);

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    Task UpdateProductAsync(Product product);

    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    Task DeleteProductAsync(int id);
}
