using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.Services
{
    /// <summary>
    /// Service for managing orders in the restaurant application.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    public class OrderService : IOrderService
    {
        private readonly RestaurantContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="context">The database context to access data.</param>
        public OrderService(RestaurantContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all orders asynchronously.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderMenuItems)
                                        .ThenInclude(omi => omi.MenuItem)
                                        .ToListAsync();
        }

        /// <summary>
        /// Retrieves an order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The order with the specified ID.</returns>
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderMenuItems)
                                        .ThenInclude(omi => omi.MenuItem)
                                        .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Adds a new order asynchronously.
        /// </summary>
        /// <param name="order">The order to add.</param>
        /// <returns>The newly added order.</returns>
        /// <exception cref="ArgumentException">Thrown when the order is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when a menu item is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when there is insufficient stock for a product.</exception>
        public async Task<Order> AddOrderAsync(Order order)
        {
            ValidateOrder(order);

            // Check if using an in-memory database to skip transactions
            var isInMemory = _context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";

            if (!isInMemory)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await ProcessOrderItems(order);
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await ProcessOrderItems(order);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            return order;
        }

        /// <summary>
        /// Updates an existing order asynchronously.
        /// </summary>
        /// <param name="order">The order with updated details.</param>
        /// <returns>The updated order.</returns>
        /// <exception cref="ArgumentException">Thrown when the order is invalid.</exception>
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            ValidateOrder(order);

            var existingOrder = await _context.Orders.FindAsync(order.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {order.Id} does not exist.");
            }

            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();

            return order;
        }

        /// <summary>
        /// Deletes an order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Validates the order using the built-in validation attributes.
        /// </summary>
        /// <param name="order">The order to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the order is invalid.</exception>
        private void ValidateOrder(Order order)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(order, null, null);
            if (!Validator.TryValidateObject(order, context, validationResults, true))
            {
                var validationMessage = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException(validationMessage);
            }

            // Additional validation for default DateTime
            if (order.DateTime == default)
            {
                throw new ArgumentException("DateTime cannot be the default value.");
            }
        }

        /// <summary>
        /// Processes the order items to ensure they exist and have sufficient stock.
        /// </summary>
        /// <param name="order">The order to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessOrderItems(Order order)
        {
            foreach (var orderMenuItem in order.OrderMenuItems)
            {
                var menuItem = await _context.MenuItems
                    .Include(mi => mi.MenuItemProducts)
                    .ThenInclude(mip => mip.Product)
                    .FirstOrDefaultAsync(mi => mi.Id == orderMenuItem.MenuItemId);

                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"MenuItem with ID {orderMenuItem.MenuItemId} not found.");
                }

                // Check stock for each product in the menu item
                foreach (var menuItemProduct in menuItem.MenuItemProducts)
                {
                    if (menuItemProduct.Product.PortionCount < 1)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product '{menuItemProduct.Product.Name}'.");
                    }
                    menuItemProduct.Product.PortionCount -= 1;
                }
            }
        }
    }
}

