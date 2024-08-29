using Xunit;
using Moq;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Tests.Services;

/// <summary>
/// Contains unit tests for the <see cref="ProductService"/> class.
/// </summary>
/// <remarks>
/// The tests in this class verify the behavior of the ProductService methods
/// for various scenarios, ensuring they perform as expected and handle errors correctly.
/// </remarks>
/// <seealso cref="ProductService"/>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>29.08.2024</date>
public class ProductServiceTests
{
    private readonly ProductService _productService;
    private readonly RestaurantContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductServiceTests"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor sets up the in-memory database context and the ProductService instance
    /// used in the test methods. An in-memory database is used to avoid any dependency on an actual database.
    /// </remarks>
    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<RestaurantContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new RestaurantContext(options);
        _productService = new ProductService(_context);
    }

    /// <summary>
    /// Tests the <see cref="ProductService.AddProductAsync(Product)"/> method.
    /// Verifies that an <see cref="ArgumentException"/> is thrown when a product with an invalid ID is added.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenProductIdIsInvalid()
    {
        // Arrange
        var invalidProduct = new Product { Id = 0, Name = "Chicken", PortionCount = 10, PortionSize = 0.3 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(invalidProduct));
    }

    /// <summary>
    /// Tests the <see cref="ProductService.AddProductAsync(Product)"/> method.
    /// Verifies that an <see cref="ArgumentException"/> is thrown when a product with an invalid name is added.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenProductNameIsInvalid()
    {
        // Arrange
        var invalidProduct = new Product { Id = 1, Name = "12345", PortionCount = 10, PortionSize = 0.3 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(invalidProduct));
    }

    /// <summary>
    /// Tests the <see cref="ProductService.AddProductAsync(Product)"/> method.
    /// Verifies that an <see cref="ArgumentException"/> is thrown when a product with an invalid portion count is added.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenPortionCountIsInvalid()
    {
        // Arrange
        var invalidProduct = new Product { Id = 1, Name = "Chicken", PortionCount = -1, PortionSize = 0.3 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(invalidProduct));
    }

    /// <summary>
    /// Tests the <see cref="ProductService.AddProductAsync(Product)"/> method.
    /// Verifies that an <see cref="ArgumentException"/> is thrown when a product with an invalid portion size is added.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenPortionSizeIsInvalid()
    {
        // Arrange
        var invalidProduct = new Product { Id = 1, Name = "Chicken", PortionCount = 10, PortionSize = -0.3 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.AddProductAsync(invalidProduct));
    }

    /// <summary>
    /// Tests the <see cref="ProductService.AddProductAsync(Product)"/> method.
    /// Verifies that a valid product is added successfully and that the returned product matches the input.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddProductAsync_ShouldAddProduct_WhenProductIsValid()
    {
        // Arrange
        var validProduct = new Product
        {
            Id = 1,
            Name = "Chicken",
            PortionCount = 10,
            PortionSize = 0.3,
            Unit = "kg"  // Ensure this is set to a valid value
        };

        // Act
        var result = await _productService.AddProductAsync(validProduct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Chicken", result.Name);
        Assert.Equal(10, result.PortionCount);
        Assert.Equal("kg", result.Unit);
    }

}
