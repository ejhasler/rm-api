using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Services
{
    /// <summary>
    /// Defines methods for managing orders within the restaurant system.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    public interface IOrderService
    {
        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A collection of all orders.</returns>
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The order with the specified ID, or null if not found.</returns>
        Task<Order> GetOrderByIdAsync(int id);

        /// <summary>
        /// Adds a new order to the system.
        /// </summary>
        /// <param name="order">The order to add.</param>
        /// <returns>The newly added order.</returns>
        Task<Order> AddOrderAsync(Order order);

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="order">The order with updated information.</param>
        /// <returns>The updated order.</returns>
        Task<Order> UpdateOrderAsync(Order order);

        /// <summary>
        /// Deletes an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteOrderAsync(int id);
    }
}
