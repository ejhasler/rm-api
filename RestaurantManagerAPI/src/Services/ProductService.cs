using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Data.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System;
using RestaurantManagerAPI.Services;

namespace RestaurantManagerAPI.Application.Services
{
    /// <summary>
    /// Service for managing products in the restaurant application.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for accessing product data.</param>
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adds a new product asynchronously.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The newly added product.</returns>
        /// <exception cref="ArgumentException">Thrown when the product is invalid.</exception>
        public async Task<Product> AddProductAsync(Product product)
        {
            ValidateProduct(product);

            await _productRepository.AddAsync(product);
            return product;
        }

        /// <summary>
        /// Updates an existing product asynchronously.
        /// </summary>
        /// <param name="product">The product with updated details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the product is invalid.</exception>
        public async Task UpdateProductAsync(Product product)
        {
            ValidateProduct(product);

            await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// Deletes a product by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Validates the product using the built-in validation attributes.
        /// </summary>
        /// <param name="product">The product to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the product is invalid.</exception>
        private void ValidateProduct(Product product)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            if (!Validator.TryValidateObject(product, context, validationResults, true))
            {
                var validationMessage = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException(validationMessage);
            }
        }
    }
}
