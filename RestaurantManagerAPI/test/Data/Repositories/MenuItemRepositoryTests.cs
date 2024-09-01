using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Repositories;
using RestaurantManagerAPI.Models;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Data.Repositories
{
    public class MenuItemRepositoryTests : IDisposable
    {
        private readonly MenuItemRepository _menuItemRepository;
        private readonly RestaurantContext _context;

        public MenuItemRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseInMemoryDatabase(databaseName: "RestaurantManagerTestDb")
                .Options;

            _context = new RestaurantContext(options);
            _menuItemRepository = new MenuItemRepository(_context);

            // Clear database before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        // Implement IDisposable to ensure context is disposed after tests
        public void Dispose()
        {
            _context.Dispose();
        }

        #region GetAllMenuItemsAsync

        [Fact]
        public async Task GetAllMenuItemsAsync_ShouldReturnAllMenuItems_WhenMenuItemsExist()
        {
            // Arrange
            _context.MenuItems.AddRange(new List<MenuItem>
            {
                new MenuItem { Id = 1, Name = "Grilled Chicken Sandwich", ProductIds = new List<int> { 1, 2 } },
                new MenuItem { Id = 2, Name = "Veggie Burger", ProductIds = new List<int> { 3, 4 } }
            });
            _context.SaveChanges();

            // Act
            var result = await _menuItemRepository.GetAllMenuItemsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllMenuItemsAsync_ShouldReturnEmptyList_WhenNoMenuItemsExist()
        {
            // Arrange
            // Ensure the context is cleared
            _context.MenuItems.RemoveRange(_context.MenuItems);
            _context.SaveChanges();

            // Act
            var result = await _menuItemRepository.GetAllMenuItemsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetMenuItemByIdAsync

        [Fact]
        public async Task GetMenuItemByIdAsync_ShouldReturnMenuItem_WhenMenuItemExists()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Grilled Chicken Sandwich", ProductIds = new List<int> { 1, 2 } };
            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            // Act
            var result = await _menuItemRepository.GetMenuItemByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetMenuItemByIdAsync_ShouldReturnNull_WhenMenuItemDoesNotExist()
        {
            // Arrange
            // Ensure the context is cleared
            _context.MenuItems.RemoveRange(_context.MenuItems);
            _context.SaveChanges();

            // Act
            var result = await _menuItemRepository.GetMenuItemByIdAsync(999); // ID that doesn't exist

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddMenuItemAsync

        [Fact]
        public async Task AddMenuItemAsync_ShouldAddMenuItem_WhenMenuItemIsValid()
        {
            // Arrange
            var menuItem = new MenuItem { Name = "New Menu Item", ProductIds = new List<int> { 1, 2 } };

            // Act
            var result = await _menuItemRepository.AddMenuItemAsync(menuItem);

            // Assert
            var addedMenuItem = await _context.MenuItems.FindAsync(menuItem.Id);
            addedMenuItem.Should().NotBeNull();
            addedMenuItem.Name.Should().Be("New Menu Item");
        }

        #endregion

        #region UpdateMenuItemAsync

        [Fact]
        public async Task UpdateMenuItemAsync_ShouldUpdateMenuItem_WhenMenuItemExists()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Old Menu Item", ProductIds = new List<int> { 1, 2 } };
            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            menuItem.Name = "Updated Menu Item";

            // Act
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);

            // Assert
            var updatedMenuItem = await _context.MenuItems.FindAsync(1);
            updatedMenuItem.Should().NotBeNull();
            updatedMenuItem.Name.Should().Be("Updated Menu Item");
        }

        #endregion

        #region DeleteMenuItemAsync

        [Fact]
        public async Task DeleteMenuItemAsync_ShouldDeleteMenuItem_WhenMenuItemExists()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 1, Name = "Menu Item to Delete", ProductIds = new List<int> { 1, 2 } };
            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            // Act
            await _menuItemRepository.DeleteMenuItemAsync(1);

            // Assert
            var deletedMenuItem = await _context.MenuItems.FindAsync(1);
            deletedMenuItem.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMenuItemAsync_ShouldNotThrow_WhenMenuItemDoesNotExist()
        {
            // Arrange
            // Ensure the context is cleared
            _context.MenuItems.RemoveRange(_context.MenuItems);
            _context.SaveChanges();

            // Act
            Func<Task> act = async () => await _menuItemRepository.DeleteMenuItemAsync(999); // ID that doesn't exist

            // Assert
            await act.Should().NotThrowAsync();
        }

        #endregion
    }
}
