using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Data.Repositories;

/// <summary>
/// Represents the interface for the repository responsible
/// for managing Product in the restaurant.
/// Provides methods for performing CRUD operations on <see cref="Product"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all Product.
    /// </summary>
    /// <returns> All the product.</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Retrieves a Product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns> The product with the specified identifier.</returns>
    Task<Product> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new Product.
    /// </summary>
    Task AddAsync(Product product);

    /// <summary>
    /// Updates an existing Product.
    /// </summary>
    Task UpdateAsync(Product product);

    /// <summary>
    /// Deletes a Product by its unique identifier.
    /// </summary>
    Task DeleteAsync(int id);
}