using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Data
{
    /// <summary>
    /// Represents the database context for the Restaurant Manager application.
    /// </summary>
    public class RestaurantContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantContext"/> class.
        /// </summary>
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the collection of products in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

    }
}
