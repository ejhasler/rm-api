using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Services;

/// <summary>
/// Provides services for managing the relationship between menu items and products.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>29.08.2021</date>
public class MenuItemProductService : IMenuItemProductService
{
    private readonly RestaurantContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemProductService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public MenuItemProductService(RestaurantContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a product to a menu item.
    /// </summary>
    /// <param name="menuItemProduct">The relationship entity to add.</param>
    public async Task AddMenuItemProductAsync(MenuItemProduct menuItemProduct)
    {
        _context.MenuItemProducts.Add(menuItemProduct);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes a product from a menu item.
    /// </summary>
    /// <param name="menuItemProduct">The relationship entity to remove.</param>
    public async Task RemoveMenuItemProductAsync(MenuItemProduct menuItemProduct)
    {
        var entity = await _context.MenuItemProducts
            .FirstOrDefaultAsync(mp => mp.MenuItemId == menuItemProduct.MenuItemId && mp.ProductId == menuItemProduct.ProductId);

        if (entity != null)
        {
            _context.MenuItemProducts.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
