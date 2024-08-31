using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.Repositories;
using RestaurantManagerAPI.Application.Services;
using RestaurantManagerAPI.Data.Repositories;
using System.Linq;

namespace RestaurantManagerAPI.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ProductService"/>.
    /// </summary>
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductServiceTests"/> class.
        /// </summary>
        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        #region Validation Tests

        [Fact]
        public async Task AddProduct_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Id = 0, Name = "Chicken", PortionCount = 1.0, Unit = "kg", PortionSize = 0.5 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(product));
            Assert.Contains("Id must be greater than 0.", exception.Message);
        }

        [Fact]
        public async Task AddProduct_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken123", PortionCount = 1.0, Unit = "kg", PortionSize = 0.5 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(product));
            Assert.Contains("Name cannot contain numbers.", exception.Message);
        }

        [Fact]
        public async Task AddProduct_InvalidPortionCount_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 0, Unit = "kg", PortionSize = 0.5 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(product));
            Assert.Contains("Portion count must be greater than 0.", exception.Message);
        }

        [Fact]
        public async Task AddProduct_InvalidPortionSize_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 1.0, Unit = "kg", PortionSize = 0 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(product));
            Assert.Contains("Portion size must be greater than 0.", exception.Message);
        }

        [Fact]
        public async Task AddProduct_NullUnit_ThrowsArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 1.0, Unit = null, PortionSize = 0.5 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(product));
            Assert.Contains("Unit is required.", exception.Message);
        }

        #endregion

        #region Service Operation Tests

        [Fact]
        public async Task GetAllProductsAsync_ReturnsProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 },
                new Product { Id = 2, Name = "Beef", PortionCount = 15, Unit = "kg", PortionSize = 0.5 }
            };

            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductExists_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Chicken", result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProductAsync_ValidProduct_AddsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductRepository.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _productService.AddProductAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(product), Times.Once);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task UpdateProductAsync_ValidProduct_UpdatesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductRepository.Setup(repo => repo.UpdateAsync(product)).Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProductAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ProductExists_DeletesProduct()
        {
            // Arrange
            var productId = 1;

            _mockProductRepository.Setup(repo => repo.DeleteAsync(productId)).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }

        #endregion
    }
}
