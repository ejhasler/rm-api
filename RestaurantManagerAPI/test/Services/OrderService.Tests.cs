using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using FluentAssertions;
using RestaurantManagerAPI.Data;

namespace RestaurantManagerAPI.Tests.Services
{
    public class OrderServiceTests : IDisposable
    {
        private readonly OrderService _orderService;
        private readonly RestaurantContext _context;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test run
                .Options;

            _context = new RestaurantContext(options);
            _orderService = new OrderService(_context);
        }

        public void Dispose()
        {
            // Cleanup resources
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region GetAllOrdersAsync

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnAllOrders_WhenOrdersExist()
        {
            // Arrange
            _context.Orders.AddRange(new List<Order>
            {
                new Order { Id = 1, DateTime = DateTime.Now },
                new Order { Id = 2, DateTime = DateTime.Now }
            });
            _context.SaveChanges();

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnEmptyList_WhenNoOrdersExist()
        {
            // Arrange
            // No setup needed as no orders are added

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetOrderByIdAsync

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, DateTime = DateTime.Now };
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Act
            var result = await _orderService.GetOrderByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            // No setup needed as no orders are added

            // Act
            var result = await _orderService.GetOrderByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddOrderAsync

        [Fact]
        public async Task AddOrderAsync_ShouldAddOrder_WhenOrderIsValid()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Id = 1,
                Name = "Test MenuItem",
                MenuItemProducts = new List<MenuItemProduct>
                {
                    new MenuItemProduct { Product = new Product { Name = "Test Product", Unit = "kg", PortionCount = 10 } }
                }
            };

            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 } }
            };

            // Act
            var result = await _orderService.AddOrderAsync(order);

            // Assert
            var addedOrder = await _context.Orders.FindAsync(order.Id);
            addedOrder.Should().NotBeNull();
            addedOrder.Id.Should().Be(order.Id);
        }

        [Fact]
        public async Task AddOrderAsync_ShouldThrowArgumentException_WhenOrderIsInvalid()
        {
            // Arrange
            var order = new Order
            {
                DateTime = DateTime.MinValue, // Invalid datetime
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 } }
            };

            // Act
            Func<Task> act = async () => await _orderService.AddOrderAsync(order);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("*cannot be the default value*");
        }

        [Fact]
        public async Task AddOrderAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 } }
            };

            // Act
            Func<Task> act = async () => await _orderService.AddOrderAsync(order);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("MenuItem with ID 1 not found.");
        }

        [Fact]
        public async Task AddOrderAsync_ShouldThrowInvalidOperationException_WhenInsufficientStock()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Id = 1,
                Name = "Test MenuItem",
                MenuItemProducts = new List<MenuItemProduct>
                {
                    new MenuItemProduct { Product = new Product { Name = "Test Product", Unit = "kg", PortionCount = 0 } } // Insufficient stock
                }
            };

            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 } }
            };

            // Act
            Func<Task> act = async () => await _orderService.AddOrderAsync(order);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Insufficient stock for product 'Test Product'."); // Updated to match the actual exception message
        }


        #endregion

        #region UpdateOrderAsync

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenOrderIsValid()
        {
            // Arrange
            var order = new Order { Id = 1, DateTime = DateTime.Now };
            _context.Orders.Add(order);
            _context.SaveChanges();

            order.DateTime = DateTime.Now.AddHours(1); // Update order time

            // Act
            var updatedOrder = await _orderService.UpdateOrderAsync(order);

            // Assert
            var dbOrder = await _context.Orders.FindAsync(order.Id);
            dbOrder.DateTime.Should().Be(order.DateTime);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldThrowArgumentException_WhenOrderIsInvalid()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                DateTime = DateTime.MinValue // Invalid datetime
            };

            // Act
            Func<Task> act = async () => await _orderService.UpdateOrderAsync(order);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("*cannot be the default value*");
        }

        #endregion

        #region DeleteOrderAsync

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, DateTime = DateTime.Now };
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Act
            await _orderService.DeleteOrderAsync(1);

            // Assert
            var deletedOrder = await _context.Orders.FindAsync(1);
            deletedOrder.Should().BeNull();
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldNotThrow_WhenOrderDoesNotExist()
        {
            // Arrange
            // No setup needed as no orders are added

            // Act
            Func<Task> act = async () => await _orderService.DeleteOrderAsync(1);

            // Assert
            await act.Should().NotThrowAsync();
        }

        #endregion
    }
}
