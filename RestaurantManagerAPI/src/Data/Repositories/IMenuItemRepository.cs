using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories;

/// <summary>
/// Represents the interface for the repository responsible
/// for managing Menu Items in the restaurant.
/// Provides methods for performing CRUD operations on <see cref="MenuItem"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public interface IMenuItemRepository
{
    /// <summary>
    /// Retrieves all Menu Items.
    /// </summary>
    /// <returns> All the menu items.</returns>
    Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();

    /// <summary>
    /// Retrieves a Menu Item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the menu item.</param>
    /// <returns> The menu item with the specified identifier.</returns>
    Task<MenuItem> GetMenuItemByIdAsync(int id);

    /// <summary>
    /// Adds a new Menu Item.
    /// </summary>
    Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);

    /// <summary>
    /// Updates an existing Menu Item.
    /// </summary>
    Task UpdateMenuItemAsync(MenuItem menuItem);

    /// <summary>
    /// Deletes a Menu Item by its unique identifier.
    /// </summary>
    Task DeleteMenuItemAsync(int id);
}
