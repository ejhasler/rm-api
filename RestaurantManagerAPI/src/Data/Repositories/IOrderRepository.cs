using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories;

/// <summary>
/// Represents the interface for the repository responsible
/// for managing Orders in the restaurant.
/// Provides methods for performing CRUD operations on <see cref="Order"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public interface IOrderRepository
{

    /// <summary>
    /// Retrieves all Orders.
    /// </summary>
    /// <returns> All the orders.</returns>
    Task<IEnumerable<Order>> GetAllOrdersAsync();

    /// <summary>
    /// Retrieves a Order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns> The order with the specified identifier.</returns>
    Task<Order> GetOrderByIdAsync(int id);

    /// <summary>
    /// Adds a new Order.
    /// </summary>
    Task<Order> AddOrderAsync(Order order);

    /// <summary>
    /// Updates an existing Order.
    /// </summary>
    Task UpdateOrderAsync(Order order);

    /// <summary>
    /// Deletes a Order by its unique identifier.
    /// </summary>
    Task DeleteOrderAsync(int id);
}
