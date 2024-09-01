using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Services
{
    /// <summary>
    /// Defines methods for managing menu items.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    public interface IMenuItemService
    {
        /// <summary>
        /// Retrieves all menu items.
        /// </summary>
        /// <returns>A collection of all menu items.</returns>
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();

        /// <summary>
        /// Retrieves a menu item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the menu item.</param>
        /// <returns>The menu item with the specified ID, or null if not found.</returns>
        Task<MenuItem> GetMenuItemByIdAsync(int id);

        /// <summary>
        /// Adds a new menu item.
        /// </summary>
        /// <param name="menuItem">The menu item to add.</param>
        /// <returns>The newly added menu item.</returns>
        Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);

        /// <summary>
        /// Updates an existing menu item.
        /// </summary>
        /// <param name="menuItem">The menu item with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateMenuItemAsync(MenuItem menuItem);

        /// <summary>
        /// Deletes a menu item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the menu item to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteMenuItemAsync(int id);
    }
}
