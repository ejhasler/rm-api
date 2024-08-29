using Xunit;
using Moq;
using RestaurantManagerAPI.Controllers;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestaurantManagerAPI.Tests.Controllers;

/// <summary>
/// Test class for the ProductsController.
/// </summary>
/// <remarks>
/// This class contains tests for the ProductsController class.
/// </remarks>
/// <seealso cref="ProductsController"/>
/// <author> Even Johan Pereira Haslerud </author>
/// <date> 29.08.2024 </date>
public class ProductsControllerTests
{
    private readonly ProductsController _controller;
    private readonly Mock<IProductService> _mockService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsControllerTests"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor sets up the mock service and the controller instance
    /// used in the test methods.
    /// </remarks>
    public ProductsControllerTests()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductsController(_mockService.Object);
    }

    /// <summary>
    /// Tests the <see cref="ProductsController.GetProduct(int)"/> method.
    /// Verifies that a BadRequest result is returned when an invalid ID (e.g., 0) is provided.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task GetProduct_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        // Act
        var result = await _controller.GetProduct(0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }


    /// <summary>
    /// Tests the <see cref="ProductsController.AddProduct(Product)"/> method.
    /// Verifies that a BadRequest result is returned when the provided model is invalid.
    /// </summary>
    /// <remarks>
    /// This test sets up a mock model state error to simulate invalid input.
    /// </remarks>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProduct_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var invalidProduct = new Product { Id = 0, Name = "1234", PortionCount = -1, PortionSize = -0.5 };
        _controller.ModelState.AddModelError("Name", "Name cannot contain numbers");

        // Act
        var result = await _controller.AddProduct(invalidProduct);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    /// <summary>
    /// Tests the <see cref="ProductsController.AddProduct(Product)"/> method.
    /// Verifies that a CreatedAtAction result is returned when a valid product is added.
    /// </summary>
    /// <remarks>
    /// This test mocks the behavior of the IProductService to return a valid product upon addition.
    /// </remarks>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProduct_ShouldReturnCreatedAtAction_WhenProductIsValid()
    {
        // Arrange
        var validProduct = new Product { Id = 1, Name = "Chicken", PortionCount = 10, PortionSize = 0.3 };
        _mockService.Setup(service => service.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(validProduct);

        // Act
        var result = await _controller.AddProduct(validProduct);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnProduct = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal("Chicken", returnProduct.Name);
    }

    /// <summary>
    /// Tests the <see cref="ProductsController.UpdateProduct(int, Product)"/> method.
    /// Verifies that a BadRequest result is returned when the ID parameter does not match the product's ID.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenIdDoesNotMatchProductId()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Chicken", PortionCount = 10, PortionSize = 0.3 };

        // Act
        var result = await _controller.UpdateProduct(2, product);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    /// <summary>
    /// Tests the <see cref="ProductsController.DeleteProduct(int)"/> method.
    /// Verifies that a BadRequest result is returned when an invalid ID (e.g., 0) is provided.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task DeleteProduct_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        // Act
        var result = await _controller.DeleteProduct(0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
