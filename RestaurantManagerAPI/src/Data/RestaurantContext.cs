using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Data;

/// <summary>
/// Represents the database context for the Restaurant Manager application.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
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

    /// <summary>
    /// Gets or sets the collection of menu items in the database.
    /// </summary>
    public DbSet<MenuItem> MenuItems { get; set; }

    /// <summary>
    /// Gets or sets the collection of menu item products in the database.
    /// </summary>
    public DbSet<MenuItemProduct> MenuItemProducts { get; set; }

    /// <summary>
    /// Gets or sets the collection of orders in the database.
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets or sets the collection of order menu items in the database.
    /// </summary>
    public DbSet<OrderMenuItem> OrderMenuItems { get; set; }

    /// <summary>
    /// Configures the schema needed for the database.
    /// </summary>
    /// <param name="modelBuilder">The builder being used
    /// to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItemProduct>()
            .HasKey(mp => new { mp.MenuItemId, mp.ProductId });

        modelBuilder.Entity<MenuItemProduct>()
            .HasOne(mp => mp.MenuItem)
            .WithMany(m => m.MenuItemProducts)
            .HasForeignKey(mp => mp.MenuItemId);

        modelBuilder.Entity<MenuItemProduct>()
            .HasOne(mp => mp.Product)
            .WithMany(p => p.MenuItemProducts)
            .HasForeignKey(mp => mp.ProductId);

        modelBuilder.Entity<OrderMenuItem>()
            .HasKey(om => new { om.OrderId, om.MenuItemId });

        modelBuilder.Entity<OrderMenuItem>()
            .HasOne(om => om.Order)
            .WithMany(o => o.OrderMenuItems)
            .HasForeignKey(om => om.OrderId);

        modelBuilder.Entity<OrderMenuItem>()
            .HasOne(om => om.MenuItem)
            .WithMany(m => m.OrderMenuItems)
            .HasForeignKey(om => om.MenuItemId);
    }
}
