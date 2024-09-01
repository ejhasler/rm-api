using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Data.Repositories;
using RestaurantManagerAPI.Models;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Data.Repositories
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ProductRepository _productRepository;
        private readonly RestaurantContext _context;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test
                .Options;

            _context = new RestaurantContext(options);
            _productRepository = new ProductRepository(_context);

            // Ensure the database is created
            _context.Database.EnsureCreated();
        }

        // Implement IDisposable to ensure context is disposed after tests
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            _context.Products.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Unit = "kg", PortionCount = 10, PortionSize = 0.5 },
                new Product { Id = 2, Name = "Product 2", Unit = "kg", PortionCount = 5, PortionSize = 0.25 }
            });
            await _context.SaveChangesAsync(); // Ensure changes are saved asynchronously

            // Act
            var result = await _productRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            // Ensure the context is cleared
            _context.Products.RemoveRange(_context.Products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Unit = "kg", PortionCount = 10, PortionSize = 0.5 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var result = await _productRepository.GetByIdAsync(999); // ID that doesn't exist

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddAsync

        [Fact]
        public async Task AddAsync_ShouldAddProduct_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Name = "New Product", Unit = "kg", PortionCount = 10, PortionSize = 0.5 };

            // Act
            await _productRepository.AddAsync(product);

            // Assert
            var addedProduct = await _context.Products.FindAsync(product.Id);
            addedProduct.Should().NotBeNull();
            addedProduct.Name.Should().Be("New Product");
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Old Product", Unit = "kg", PortionCount = 10, PortionSize = 0.5 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            product.Name = "Updated Product";

            // Act
            await _productRepository.UpdateAsync(product);

            // Assert
            var updatedProduct = await _context.Products.FindAsync(1);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Name.Should().Be("Updated Product");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new Product { Id = 999, Name = "Non-Existent Product", Unit = "kg", PortionCount = 10, PortionSize = 0.5 };

            // Act
            Func<Task> act = async () => await _productRepository.UpdateAsync(product);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Product with ID 999 not found for update.");
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product to Delete", Unit = "kg", PortionCount = 10, PortionSize = 0.5 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            await _productRepository.DeleteAsync(1);

            // Assert
            var deletedProduct = await _context.Products.FindAsync(1);
            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotThrow_WhenProductDoesNotExist()
        {
            // Act
            Func<Task> act = async () => await _productRepository.DeleteAsync(999); // ID that doesn't exist

            // Assert
            await act.Should().NotThrowAsync();
        }

        #endregion
    }
}
