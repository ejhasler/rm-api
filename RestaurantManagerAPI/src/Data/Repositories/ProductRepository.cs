using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Data.Repositories
{
    /// <summary>
    /// Repository class for managing <see cref="Product"/> entities.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    public class ProductRepository : IProductRepository
    {
        private readonly RestaurantContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data access.</param>
        public ProductRepository(RestaurantContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a list of all <see cref="Product"/> entities.</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the <see cref="Product"/> entity with the specified ID.</returns>
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Adds a new product to the database asynchronously.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the database asynchronously.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> entity with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateAsync(Product product)
        {
            // Check if the product exists before attempting to update it
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found for update.");
            }
            
        }

        /// <summary>
        /// Deletes a product by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
