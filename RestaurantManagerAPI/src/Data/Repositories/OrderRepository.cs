using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories;

/// <summary>
/// Repository class for managing <see cref="Order"/> entities.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class OrderRepository : IOrderRepository
{
    private readonly RestaurantContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to use for data access.</param>
    public OrderRepository(RestaurantContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all orders from the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation,
    /// containing a list of all <see cref="Order"/> entities.</returns>
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.Include(o => o.OrderMenuItems)
                                    .ThenInclude(omi => omi.MenuItem)
                                    .ToListAsync();
    }

    /// <summary>
    /// Retrieves an order by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing
    /// the <see cref="Order"/> entity with the specified ID.</returns>
    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _context.Orders.Include(o => o.OrderMenuItems)
                                    .ThenInclude(omi => omi.MenuItem)
                                    .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <summary>
    /// Adds a new order to the database asynchronously.
    /// </summary>
    /// <param name="order">The <see cref="Order"/> entity to add.</param>
    /// <returns>A task that represents the asynchronous operation, containing
    /// the newly added <see cref="Order"/> entity.</returns>
    public async Task<Order> AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// Updates an existing order in the database asynchronously.
    /// </summary>
    /// <param name="order">The <see cref="Order"/> entity with updated information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateOrderAsync(Order order)
    {
        // Check if the order exists before attempting to update it
        var existingOrder = await _context.Orders.FindAsync(order.Id);
        if (existingOrder != null)
        {
            // Update the existing order's properties
            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
        }
        else
        {
            // Optionally, log a warning or throw a custom exception
            throw new KeyNotFoundException($"Order with ID {order.Id} not found for update.");
        }
    }


    /// <summary>
    /// Deletes an order by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the order to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
