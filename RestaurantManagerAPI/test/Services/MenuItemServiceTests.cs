using Moq;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Repositories;
using RestaurantManagerAPI.Services;
using FluentAssertions;
using System.Reflection;

namespace RestaurantManagerAPI.Tests.Services
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

        #region GetAllMenuItemsAsync

        [Fact]
        public async Task GetAllMenuItemsAsync_MenuItemsExist_ShouldReturnAllMenuItems()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Pasta" },
                new MenuItem { Id = 2, Name = "Salad" }
            };
            _mockMenuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync(menuItems);

            // Act
            var result = await _menuItemService.GetAllMenuItemsAsync();

            // Assert
            result.Should().BeEquivalentTo(menuItems);
        }

        [Fact]
        public async Task GetAllMenuItemsAsync_NoMenuItemsExist_ShouldReturnEmptyList()
        {
            // Arrange
            _mockMenuItemRepository.Setup(repo => repo.GetAllMenuItemsAsync()).ReturnsAsync(new List<MenuItem>());

            // Act
            var result = await _menuItemService.GetAllMenuItemsAsync();

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region GetMenuItemByIdAsync

        [Fact]
        public async Task GetMenuItemByIdAsync_MenuItemExists_ShouldReturnMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta" };
            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync(menuItem);

            // Act
            var result = await _menuItemService.GetMenuItemByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(menuItem);
        }

        [Fact]
        public async Task GetMenuItemByIdAsync_MenuItemDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync((MenuItem)null);

            // Act
            var result = await _menuItemService.GetMenuItemByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddMenuItemAsync

        [Fact]
        public async Task AddMenuItemAsync_ValidMenuItem_ShouldAddMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1 } };
            var product = new Product { Id = 1, Name = "Product1", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);
            _mockMenuItemRepository.Setup(repo => repo.AddMenuItemAsync(menuItem)).ReturnsAsync(menuItem);

            // Act
            var result = await _menuItemService.AddMenuItemAsync(menuItem);

            // Assert
            _mockMenuItemRepository.Verify(repo => repo.AddMenuItemAsync(menuItem), Times.Once);
            result.Should().BeEquivalentTo(menuItem);
        }

        [Fact]
        public async Task AddMenuItemAsync_InvalidMenuItem_ShouldThrowArgumentException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta123", ProductIds = new List<int> { 1 } };

            // Act
            Func<Task> act = async () => await _menuItemService.AddMenuItemAsync(menuItem);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Name cannot contain numbers.*");

            _mockMenuItemRepository.Verify(repo => repo.AddMenuItemAsync(It.IsAny<MenuItem>()), Times.Never);
        }


        [Fact]
        public async Task AddMenuItemAsync_ProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 999 } };

            _mockProductService.Setup(service => service.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            // Act
            Func<Task> act = async () => await _menuItemService.AddMenuItemAsync(menuItem);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("*Product with ID 999 does not exist.*");
            _mockMenuItemRepository.Verify(repo => repo.AddMenuItemAsync(It.IsAny<MenuItem>()), Times.Never);
        }

        #endregion

        #region UpdateMenuItemAsync

        [Fact]
        public async Task UpdateMenuItemAsync_ValidMenuItem_ShouldUpdateMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1 } };
            var existingMenuItem = new MenuItem { Id = 1, Name = "Old Pasta" };
            var product = new Product { Id = 1, Name = "Product1", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync(existingMenuItem);
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            await _menuItemService.UpdateMenuItemAsync(menuItem);

            // Assert
            _mockMenuItemRepository.Verify(repo => repo.UpdateMenuItemAsync(It.Is<MenuItem>(m => m.Name == "Pasta" && m.MenuItemProducts.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateMenuItemAsync_InvalidMenuItem_ShouldThrowArgumentException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta123", ProductIds = new List<int> { 1 } };

            // Act
            Func<Task> act = async () => await _menuItemService.UpdateMenuItemAsync(menuItem);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Name cannot contain numbers.*");

            _mockMenuItemRepository.Verify(repo => repo.UpdateMenuItemAsync(It.IsAny<MenuItem>()), Times.Never);
        }

        [Fact]
        public async Task UpdateMenuItemAsync_MenuItemDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1 } };

            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync((MenuItem)null);

            // Act
            Func<Task> act = async () => await _menuItemService.UpdateMenuItemAsync(menuItem);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("*MenuItem with ID 1 does not exist.*");
            _mockMenuItemRepository.Verify(repo => repo.UpdateMenuItemAsync(It.IsAny<MenuItem>()), Times.Never);
        }

        [Fact]
        public async Task UpdateMenuItemAsync_ProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 999 } };
            var existingMenuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1 } };

            _mockMenuItemRepository.Setup(repo => repo.GetMenuItemByIdAsync(1)).ReturnsAsync(existingMenuItem);
            _mockProductService.Setup(service => service.GetProductByIdAsync(999)).ReturnsAsync((Product)null);

            // Act
            Func<Task> act = async () => await _menuItemService.UpdateMenuItemAsync(menuItem);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("*Product with ID 999 does not exist.*");
            _mockMenuItemRepository.Verify(repo => repo.UpdateMenuItemAsync(It.IsAny<MenuItem>()), Times.Never);
        }

        #endregion

        #region DeleteMenuItemAsync

        [Fact]
        public async Task DeleteMenuItemAsync_MenuItemExists_ShouldDeleteMenuItem()
        {
            // Arrange
            var menuItemId = 1;

            // Act
            await _menuItemService.DeleteMenuItemAsync(menuItemId);

            // Assert
            _mockMenuItemRepository.Verify(repo => repo.DeleteMenuItemAsync(menuItemId), Times.Once);
        }

        [Fact]
        public async Task DeleteMenuItemAsync_MenuItemDoesNotExist_ShouldHandleGracefully()
        {
            // Arrange
            var menuItemId = 1;
            _mockMenuItemRepository.Setup(repo => repo.DeleteMenuItemAsync(menuItemId)).Throws(new KeyNotFoundException());

            // Act
            Func<Task> act = async () => await _menuItemService.DeleteMenuItemAsync(menuItemId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        #endregion

        #region ValidateMenuItem

        [Fact]
        public void ValidateMenuItem_InvalidName_ShouldThrowArgumentException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta123", ProductIds = new List<int> { 1 } };

            // Act
            Action act = () => _menuItemService.GetType()
                                               .GetMethod("ValidateMenuItem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                               .Invoke(_menuItemService, new object[] { menuItem });

            // Assert
            act.Should().Throw<TargetInvocationException>().WithInnerException<ArgumentException>().WithMessage("*Name cannot contain numbers.*");
        }

        [Fact]
        public void ValidateMenuItem_ValidMenuItem_ShouldNotThrowException()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Pasta", ProductIds = new List<int> { 1 } };

            // Act
            Action act = () => _menuItemService.GetType()
                                               .GetMethod("ValidateMenuItem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                               .Invoke(_menuItemService, new object[] { menuItem });

            // Assert
            act.Should().NotThrow();
        }

        #endregion
    }
}
