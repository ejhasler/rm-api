using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Services;

/// <summary>
/// Represents a service for managing products in the restaurant stock.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>29.08.2021</date>
public class ProductService : IProductService
{
    private readonly RestaurantContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    public ProductService(RestaurantContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all products from the database.
    /// </summary>
    /// <returns>All the products from the database.</returns>
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product with the given id.</returns>
    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    public async Task<Product> AddProductAsync(Product product)
    {
        ValidateProduct(product); // Validate before any database operation

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    public async Task UpdateProductAsync(Product product)
    {
        ValidateProduct(product); // Validate before any database operation

        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Validates the product before any database operation.
    /// </summary>
    /// <param name="product">The product to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the product is invalid.</exception>
    private void ValidateProduct(Product product)
    {
        if (product.Id <= 0)
            throw new ArgumentException("Product ID must be greater than 0.", nameof(product.Id));

        if (string.IsNullOrWhiteSpace(product.Name) || !Regex.IsMatch(product.Name, @"^[a-zA-Z\s]+$"))
            throw new ArgumentException("Product name is invalid. It must not contain numbers.", nameof(product.Name));

        if (product.PortionCount <= 0)
            throw new ArgumentException("Portion count must be greater than 0.", nameof(product.PortionCount));

        if (product.PortionSize <= 0)
            throw new ArgumentException("Portion size must be greater than 0.", nameof(product.PortionSize));

        if (string.IsNullOrWhiteSpace(product.Unit))
            throw new ArgumentException("Unit is required.", nameof(product.Unit));
    }
}
