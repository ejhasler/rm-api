using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagerAPI.Controllers;
using RestaurantManagerAPI.DTOs;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IProductService> _mockProductService;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        #region GetProducts

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WithListOfProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", PortionCount = 10, Unit = "kg", PortionSize = 0.5 },
                new Product { Id = 2, Name = "Product 2", PortionCount = 5, Unit = "kg", PortionSize = 0.25 }
            };
            _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnProducts = okResult.Value as IEnumerable<ProductReadDto>;
            returnProducts.Should().NotBeNull();
            returnProducts.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WithEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnProducts = okResult.Value as IEnumerable<ProductReadDto>;
            returnProducts.Should().NotBeNull();
            returnProducts.Should().BeEmpty();
        }

        #endregion

        #region GetProduct

        [Fact]
        public async Task GetProduct_ShouldReturnOk_WithProduct_WhenValidIdIsProvided()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnProduct = okResult.Value as ProductReadDto;
            returnProduct.Should().NotBeNull();
            returnProduct.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnBadRequest_WhenInvalidIdIsProvided()
        {
            // Act
            var result = await _controller.GetProduct(0);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        #endregion

        #region AddProduct

        [Fact]
        public async Task AddProduct_ShouldReturnCreatedAtAction_WithNewProduct_WhenValidDtoIsProvided()
        {
            // Arrange
            var productCreateDto = new ProductCreateDto { Name = "New Product", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };
            var newProduct = new Product { Id = 1, Name = "New Product", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductService.Setup(service => service.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(newProduct);

            // Act
            var result = await _controller.AddProduct(productCreateDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201);

            var returnProduct = createdAtActionResult.Value as ProductReadDto;
            returnProduct.Should().NotBeNull();
            returnProduct.Id.Should().Be(1);
        }

        [Fact]
        public async Task AddProduct_ShouldReturnBadRequest_WhenInvalidDtoIsProvided()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.AddProduct(new ProductCreateDto());

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        #endregion

        #region UpdateProduct

        [Fact]
        public async Task UpdateProduct_ShouldReturnOk_WithUpdatedProduct_WhenValidUpdate()
        {
            // Arrange
            var productUpdateDto = new ProductUpdateDto { Id = 1, Name = "Updated Product", PortionCount = 15, Unit = "kg", PortionSize = 0.75 };
            var existingProduct = new Product { Id = 1, Name = "Old Product", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(service => service.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct(1, productUpdateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnProduct = okResult.Value as ProductReadDto;
            returnProduct.Should().NotBeNull();
            returnProduct.Name.Should().Be("Updated Product");
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var productUpdateDto = new ProductUpdateDto { Id = 2, Name = "Updated Product", PortionCount = 15, Unit = "kg", PortionSize = 0.75 };

            // Act
            var result = await _controller.UpdateProduct(1, productUpdateDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productUpdateDto = new ProductUpdateDto { Id = 1, Name = "Updated Product", PortionCount = 15, Unit = "kg", PortionSize = 0.75 };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct(1, productUpdateDto);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        #endregion

        #region DeleteProduct

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenProductExists()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Product to Delete", PortionCount = 10, Unit = "kg", PortionSize = 0.5 };

            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(service => service.DeleteProductAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnBadRequest_WhenInvalidIdIsProvided()
        {
            // Act
            var result = await _controller.DeleteProduct(0);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        #endregion
    }
}
