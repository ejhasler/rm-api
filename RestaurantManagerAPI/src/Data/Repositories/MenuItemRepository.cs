using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories;

/// <summary>
/// Represents the repository for managing
/// <see cref="MenuItem"/> entities.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class MenuItemRepository : IMenuItemRepository
{
    private readonly RestaurantContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="RestaurantContext"/> to access
    /// the database.</param>
    public MenuItemRepository(RestaurantContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all Menu Items asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing
    /// all the <see cref="MenuItem"/> entities.</returns>
    public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
    {
        return await _context.MenuItems
            .Include(mi => mi.MenuItemProducts)
            .ThenInclude(mp => mp.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a menu item by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the menu item.</param>
    /// <returns>A task representing the asynchronous operation, containing
    /// the <see cref="MenuItem"/> with the specified ID.</returns>
    public async Task<MenuItem> GetMenuItemByIdAsync(int id)
    {
        return await _context.MenuItems
            .Include(mi => mi.MenuItemProducts)
            .ThenInclude(mp => mp.Product)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }

    /// <summary>
    /// Adds a new Menu Item to the repository asynchronously.
    /// </summary>
    /// <param name="menuItem">The <see cref="MenuItem"/> to add.</param>
    /// <returns>A task representing the asynchronous operation, containing
    /// the newly added <see cref="MenuItem"/>.</returns>
    public async Task<MenuItem> AddMenuItemAsync(MenuItem menuItem)
    {
        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
        return menuItem;
    }

    /// <summary>
    /// Updates an existing menu item in the repository asynchronously.
    /// </summary>
    /// <param name="menuItem">The <see cref="MenuItem"/> with updated details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateMenuItemAsync(MenuItem menuItem)
    {
        _context.Entry(menuItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a Menu Item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the menu item to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteMenuItemAsync(int id)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
