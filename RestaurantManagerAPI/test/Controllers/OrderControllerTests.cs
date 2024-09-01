using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagerAPI.Controllers;
using RestaurantManagerAPI.DTOs;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<IOrderService> _mockOrderService;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
        }

        #region GetAllOrders

        [Fact]
        public async Task GetAllOrders_ShouldReturnOk_WithListOfOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 } } },
                new Order { Id = 2, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 2 } } }
            };
            _mockOrderService.Setup(service => service.GetAllOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnOrders = okResult.Value as IEnumerable<OrderReadDto>;
            returnOrders.Should().NotBeNull();
            returnOrders.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOk_WithEmptyList_WhenNoOrdersExist()
        {
            // Arrange
            _mockOrderService.Setup(service => service.GetAllOrdersAsync()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnOrders = okResult.Value as IEnumerable<OrderReadDto>;
            returnOrders.Should().NotBeNull();
            returnOrders.Should().BeEmpty();
        }

        #endregion

        #region GetOrder

        [Fact]
        public async Task GetOrder_ShouldReturnOk_WithOrder_WhenValidIdIsProvided()
        {
            // Arrange
            var order = new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 } } };
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrder(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnOrder = okResult.Value as OrderReadDto;
            returnOrder.Should().NotBeNull();
            returnOrder.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrder(1);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        #endregion

        #region AddOrder

        [Fact]
        public async Task AddOrder_ShouldReturnCreatedAtAction_WithNewOrder_WhenValidDtoIsProvided()
        {
            // Arrange
            var orderCreateDto = new OrderCreateDto { DateTime = DateTime.Now, MenuItemIds = new List<int> { 1, 2 } };
            var newOrder = new Order { Id = 1, DateTime = orderCreateDto.DateTime, OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 }, new OrderMenuItem { MenuItemId = 2 } } };

            _mockOrderService.Setup(service => service.AddOrderAsync(It.IsAny<Order>())).ReturnsAsync(newOrder);

            // Act
            var result = await _controller.AddOrder(orderCreateDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201);

            var returnOrder = createdAtActionResult.Value as OrderReadDto;
            returnOrder.Should().NotBeNull();
            returnOrder.Id.Should().Be(1);
        }

        #endregion

        #region UpdateOrder

        [Fact]
        public async Task UpdateOrder_ShouldReturnOk_WhenOrderIsUpdated()
        {
            // Arrange
            var orderUpdateDto = new OrderUpdateDto { Id = 1, DateTime = DateTime.Now.AddHours(1), MenuItemIds = new List<int> { 1, 2 } };
            var existingOrder = new Order { Id = 1, DateTime = DateTime.Now, OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem { MenuItemId = 1 }, new OrderMenuItem { MenuItemId = 2 } } };

            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(existingOrder);
            _mockOrderService.Setup(service => service.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(existingOrder);

            // Act
            var result = await _controller.UpdateOrder(1, orderUpdateDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var orderUpdateDto = new OrderUpdateDto { Id = 2, DateTime = DateTime.Now.AddHours(1), MenuItemIds = new List<int> { 1, 2 } };

            // Act
            var result = await _controller.UpdateOrder(1, orderUpdateDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderUpdateDto = new OrderUpdateDto { Id = 1, DateTime = DateTime.Now.AddHours(1), MenuItemIds = new List<int> { 1, 2 } };
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.UpdateOrder(1, orderUpdateDto);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("Order not found.");
        }

        #endregion

        #region DeleteOrder

        [Fact]
        public async Task DeleteOrder_ShouldReturnNoContent_WhenOrderIsDeleted()
        {
            // Arrange
            _mockOrderService.Setup(service => service.DeleteOrderAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderService.Setup(service => service.DeleteOrderAsync(1)).ThrowsAsync(new KeyNotFoundException("Order not found."));

            // Act
            Func<Task> act = async () => await _controller.DeleteOrder(1);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Order not found.");
        }

        #endregion
    }
}
