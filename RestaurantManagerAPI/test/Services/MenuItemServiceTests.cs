using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.Repositories;
using System.Linq;

namespace RestaurantManagerAPI.Tests
{
    public class MenuItemServiceTests
    {
        private readonly Mock<IMenuItemRepository> _mockMenuItemRepository;
        private readonly Mock<IProductService> _mockProductService;
        private readonly MenuItemService _menuItemService;

        public MenuItemServiceTests()
        {
            _mockMenuItemRepository = new Mock<IMenuItemRepository>();
            _mockProductService = new Mock<IProductService>();
            _menuItemService = new MenuItemService(_mockMenuItemRepository.Object, _mockProductService.Object);
        }

        #region Validation Tests

        [Fact]
        public async Task AddMenuItem_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 0, Name = "Pasta", ProductIds = new List<int> { 1 } };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _menuItemService.AddMenuItemAsync(menuItem));
            Assert.Contains("Id must be greater than 0.", exception.Message);
        }

        [Fact]
        public async Task AddMenuItem_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta123", ProductIds = new List<int> { 1 } };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _menuItemService.AddMenuItemAsync(menuItem));
            Assert.Contains("Name cannot contain numbers.", exception.Message);
        }

        [Fact]
        public async Task AddMenuItem_NonExistentProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 999 } };
            _mockProductService.Setup(service => service.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _menuItemService.AddMenuItemAsync(menuItem));
            Assert.Equal("Product with ID 999 does not exist.", exception.Message);
        }

        #endregion

        #region Service Operation Tests

        [Fact]
        public async Task GetAllMenuItemsAsync_ReturnsMenuItems()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1, 2 } },
                new MenuItem { Id = 2, Name = "Salad", ProductIds = new List<int> { 3 } }
            };

            _mockMenuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync(menuItems);

            // Act
            var result = await _menuItemService.GetAllMenuItemsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetMenuItemByIdAsync_MenuItemExists_ReturnsMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1, 2 } };

            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync(menuItem);

            // Act
            var result = await _menuItemService.GetMenuItemByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Pasta", result.Name);
        }

        [Fact]
        public async Task GetMenuItemByIdAsync_MenuItemDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync((MenuItem)null);

            // Act
            var result = await _menuItemService.GetMenuItemByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddMenuItemAsync_ValidMenuItem_AddsMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1, 2 } };

            _mockProductService.Setup(service => service.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockMenuItemRepository.Setup(repo => repo.AddMenuItemAsync(menuItem)).ReturnsAsync(menuItem);

            // Act
            var result = await _menuItemService.AddMenuItemAsync(menuItem);

            // Assert
            _mockMenuItemRepository.Verify(repo => repo.AddMenuItemAsync(menuItem), Times.Once);
            Assert.Equal(menuItem, result);
        }

        [Fact]
        public async Task UpdateMenuItemAsync_ValidMenuItem_UpdatesMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1, 2 } };

            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(menuItem.Id)).ReturnsAsync(menuItem);
            _mockProductService.Setup(service => service.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockMenuItemRepository.Setup(repo => repo.UpdateMenuItemAsync(menuItem)).Returns(Task.CompletedTask);

            // Act
            await _menuItemService.UpdateMenuItemAsync(menuItem);

            // Assert
            _mockMenuItemRepository.Verify(repo => repo.UpdateMenuItemAsync(menuItem), Times.Once);
        }

        [Fact]
        public async Task DeleteMenuItemAsync_MenuItemExists_DeletesMenuItem()
        {
            // Arrange
            var menuItemId = 1;

            _mockMenuItemRepository.Setup(repo => repo.DeleteMenuItemAsync(menuItemId)).Returns(Task.CompletedTask);

            // Act
            await _menuItemService.DeleteMenuItemAsync(menuItemId);

            // Assert
            _mockMenuItemRepository.Verify(repo => repo.DeleteMenuItemAsync(menuItemId), Times.Once);
        }

        #endregion
    }
}
