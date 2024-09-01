using Moq;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Data.Repositories;
using FluentAssertions;
using System.Reflection;
using RestaurantManagerAPI.Application.Services;

namespace RestaurantManagerAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        #region GetAllProductsAsync

        [Fact]
        public async Task GetAllProductsAsync_ProductsExist_ShouldReturnAllProducts()
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
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetAllProductsAsync_NoProductsExist_ShouldReturnEmptyList()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region GetProductByIdAsync

        [Fact]
        public async Task GetProductByIdAsync_ProductExists_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddProductAsync

        [Fact]
        public async Task AddProductAsync_ValidProduct_ShouldAddProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            // Act
            var result = await _productService.AddProductAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(product), Times.Once);
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task AddProductAsync_InvalidProduct_ShouldThrowArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken123", PortionCount = -1, Unit = "kg", PortionSize = 0 };

            // Act
            Func<Task> act = async () => await _productService.AddProductAsync(product);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Name cannot contain numbers.*, Portion count must be greater than 0.*, Portion size must be greater than 0.*");

            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        #endregion

        #region UpdateProductAsync

        [Fact]
        public async Task UpdateProductAsync_ValidProduct_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            // Act
            await _productService.UpdateProductAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(product), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_InvalidProduct_ShouldThrowArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken123", PortionCount = -1, Unit = "kg", PortionSize = 0 };

            // Act
            Func<Task> act = async () => await _productService.UpdateProductAsync(product);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Name cannot contain numbers.*, Portion count must be greater than 0.*, Portion size must be greater than 0.*");

            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        #endregion

        #region DeleteProductAsync

        [Fact]
        public async Task DeleteProductAsync_ProductExists_ShouldDeleteProduct()
        {
            // Arrange
            var productId = 1;

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ProductDoesNotExist_ShouldHandleGracefully()
        {
            // Arrange
            var productId = 1;
            _mockProductRepository.Setup(repo => repo.DeleteAsync(productId)).Throws(new KeyNotFoundException());

            // Act
            Func<Task> act = async () => await _productService.DeleteProductAsync(productId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        #endregion

        #region ValidateProduct

        [Fact]
        public void ValidateProduct_InvalidName_ShouldThrowArgumentException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken123", PortionCount = 1.0, Unit = "kg", PortionSize = 0.5 };

            // Act
            Action act = () => _productService.GetType()
                                               .GetMethod("ValidateProduct", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                               .Invoke(_productService, new object[] { product });

            // Assert
            act.Should().Throw<TargetInvocationException>().WithInnerException<ArgumentException>().WithMessage("*Name cannot contain numbers.*");
        }

        [Fact]
        public void ValidateProduct_ValidProduct_ShouldNotThrowException()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Chicken", PortionCount = 1.0, Unit = "kg", PortionSize = 0.5 };

            // Act
            Action act = () => _productService.GetType()
                                               .GetMethod("ValidateProduct", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                               .Invoke(_productService, new object[] { product });

            // Assert
            act.Should().NotThrow();
        }

        #endregion
    }
}
