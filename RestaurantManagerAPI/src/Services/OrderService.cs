using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace RestaurantManagerAPI.Services
{
    /// <summary>
    /// Service for managing orders in the restaurant application.
    /// </summary>
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

            // Start a transaction to ensure atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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
                            await transaction.RollbackAsync();
                            throw new InvalidOperationException($"Insufficient stock for product '{menuItemProduct.Product.Name}'.");
                        }

                        // Deduct the stock
                        menuItemProduct.Product.PortionCount -= 1;
                    }
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates an existing order asynchronously.
        /// </summary>
        /// <param name="order">The order with updated details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the order is invalid.</exception>
        public async Task UpdateOrderAsync(Order order)
        {
            ValidateOrder(order);

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
        }
    }
}
