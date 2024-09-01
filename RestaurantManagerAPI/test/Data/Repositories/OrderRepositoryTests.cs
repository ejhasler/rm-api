using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Repositories;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Data.Repositories
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly OrderRepository _orderRepository;
        private readonly RestaurantContext _context;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test
                .Options;

            _context = new RestaurantContext(options);
            _orderRepository = new OrderRepository(_context);

            // Ensure the database is created
            _context.Database.EnsureCreated();
        }

        // Implement IDisposable to ensure context is disposed after tests
        public void Dispose()
        {
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
                new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() },
                new Order { Id = 2, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() }
            });
            await _context.SaveChangesAsync(); // Ensure changes are saved asynchronously

            // Act
            var result = await _orderRepository.GetAllOrdersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnEmptyList_WhenNoOrdersExist()
        {
            // Arrange
            // Ensure the context is cleared
            _context.Orders.RemoveRange(_context.Orders);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetAllOrdersAsync();

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
            var order = new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetOrderByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Act
            var result = await _orderRepository.GetOrderByIdAsync(999); // ID that doesn't exist

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddOrderAsync

        [Fact]
        public async Task AddOrderAsync_ShouldAddOrder_WhenOrderIsValid()
        {
            // Arrange
            var order = new Order { DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() };

            // Act
            var result = await _orderRepository.AddOrderAsync(order);

            // Assert
            var addedOrder = await _context.Orders.FindAsync(order.Id);
            addedOrder.Should().NotBeNull();
            addedOrder.Id.Should().Be(order.Id);
        }

        #endregion

        #region UpdateOrderAsync

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            order.DateTime = DateTime.Now.AddHours(1); // Update the DateTime

            // Act
            await _orderRepository.UpdateOrderAsync(order);

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(1);
            updatedOrder.Should().NotBeNull();
            updatedOrder.DateTime.Should().BeCloseTo(order.DateTime, TimeSpan.FromSeconds(1));
        }

        #endregion

        #region UpdateOrderAsync

        [Fact]
        public async Task UpdateOrderAsync_ShouldThrowKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var order = new Order { Id = 999, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() };

            // Act
            Func<Task> act = async () => await _orderRepository.UpdateOrderAsync(order);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Order with ID 999 not found for update.");
        }

        #endregion

        #region DeleteOrderAsync

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem>() };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _orderRepository.DeleteOrderAsync(1);

            // Assert
            var deletedOrder = await _context.Orders.FindAsync(1);
            deletedOrder.Should().BeNull();
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldNotThrow_WhenOrderDoesNotExist()
        {
            // Act
            Func<Task> act = async () => await _orderRepository.DeleteOrderAsync(999); // ID that doesn't exist

            // Assert
            await act.Should().NotThrowAsync();
        }

        #endregion
    }
}
