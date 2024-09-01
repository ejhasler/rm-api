using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagerAPI.Controllers;
using RestaurantManagerAPI.DTOs;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Controllers
{
    public class MenuItemsControllerTests
    {
        private readonly MenuItemsController _controller;
        private readonly Mock<IMenuItemService> _mockMenuItemService;

        public MenuItemsControllerTests()
        {
            _mockMenuItemService = new Mock<IMenuItemService>();
            _controller = new MenuItemsController(_mockMenuItemService.Object);
        }

        #region GetAllMenuItems

        [Fact]
        public async Task GetAllMenuItems_ShouldReturnOk_WithListOfMenuItems_WhenMenuItemsExist()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "MenuItem 1", MenuItemProducts = new List<MenuItemProduct> { new MenuItemProduct { ProductId = 1 } } },
                new MenuItem { Id = 2, Name = "MenuItem 2", MenuItemProducts = new List<MenuItemProduct> { new MenuItemProduct { ProductId = 2 } } }
            };
            _mockMenuItemService.Setup(service => service.GetAllMenuItemsAsync()).ReturnsAsync(menuItems);

            // Act
            var result = await _controller.GetAllMenuItems();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnMenuItems = okResult.Value as IEnumerable<MenuItemReadDto>;
            returnMenuItems.Should().NotBeNull();
            returnMenuItems.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllMenuItems_ShouldReturnOk_WithEmptyList_WhenNoMenuItemsExist()
        {
            // Arrange
            _mockMenuItemService.Setup(service => service.GetAllMenuItemsAsync()).ReturnsAsync(new List<MenuItem>());

            // Act
            var result = await _controller.GetAllMenuItems();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnMenuItems = okResult.Value as IEnumerable<MenuItemReadDto>;
            returnMenuItems.Should().NotBeNull();
            returnMenuItems.Should().BeEmpty();
        }

        #endregion

        #region GetMenuItem

        [Fact]
        public async Task GetMenuItem_ShouldReturnOk_WithMenuItem_WhenValidIdIsProvided()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "MenuItem 1", MenuItemProducts = new List<MenuItemProduct> { new MenuItemProduct { ProductId = 1 } } };
            _mockMenuItemService.Setup(service => service.GetMenuItemByIdAsync(1)).ReturnsAsync(menuItem);

            // Act
            var result = await _controller.GetMenuItem(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnMenuItem = okResult.Value as MenuItemReadDto;
            returnMenuItem.Should().NotBeNull();
            returnMenuItem.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetMenuItem_ShouldReturnNotFound_WhenMenuItemDoesNotExist()
        {
            // Arrange
            _mockMenuItemService.Setup(service => service.GetMenuItemByIdAsync(1)).ReturnsAsync((MenuItem)null);

            // Act
            var result = await _controller.GetMenuItem(1);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        #endregion

        #region AddMenuItem

        [Fact]
        public async Task AddMenuItem_ShouldReturnCreatedAtAction_WithNewMenuItem_WhenValidDtoIsProvided()
        {
            // Arrange
            var menuItemCreateDto = new MenuItemCreateDto { Name = "New MenuItem", ProductIds = new List<int> { 1, 2 } };
            var newMenuItem = new MenuItem { Id = 1, Name = "New MenuItem", MenuItemProducts = new List<MenuItemProduct> { new MenuItemProduct { ProductId = 1 }, new MenuItemProduct { ProductId = 2 } } };

            _mockMenuItemService.Setup(service => service.AddMenuItemAsync(It.IsAny<MenuItem>())).ReturnsAsync(newMenuItem);

            // Act
            var result = await _controller.AddMenuItem(menuItemCreateDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201);

            var returnMenuItem = createdAtActionResult.Value as MenuItemReadDto;
            returnMenuItem.Should().NotBeNull();
            returnMenuItem.Id.Should().Be(1);
        }

        [Fact]
        public async Task AddMenuItem_ShouldReturnBadRequest_WhenInvalidDtoIsProvided()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.AddMenuItem(new MenuItemCreateDto());

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        #endregion

        #region UpdateMenuItem

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnOk_WhenMenuItemIsUpdated()
        {
            // Arrange
            var menuItemUpdateDto = new MenuItemUpdateDto { Id = 1, Name = "Updated MenuItem", ProductIds = new List<int> { 1, 2 } };
            _mockMenuItemService.Setup(service => service.UpdateMenuItemAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateMenuItem(1, menuItemUpdateDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var menuItemUpdateDto = new MenuItemUpdateDto { Id = 2, Name = "Updated MenuItem", ProductIds = new List<int> { 1, 2 } };

            // Act
            var result = await _controller.UpdateMenuItem(1, menuItemUpdateDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        #endregion

        #region DeleteMenuItem

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnNoContent_WhenMenuItemIsDeleted()
        {
            // Arrange
            _mockMenuItemService.Setup(service => service.DeleteMenuItemAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMenuItem(1);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        #endregion
    }
}
