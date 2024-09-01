using RestaurantManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Services;

/// <summary>
/// Represents an interface for a menu item product service.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>31.08.2024</date>
public interface IMenuItemProductService
{
    /// <summary>
    /// Adds a product to a menu item.
    /// </summary>
    /// <param name="menuItemProduct">The relationship entity to add.</param>
    Task AddMenuItemProductAsync(MenuItemProduct menuItemProduct);

    /// <summary>
    /// Removes a product from a menu item.
    /// </summary>
    /// <param name="menuItemProduct">The relationship entity to remove.</param>
    Task RemoveMenuItemProductAsync(MenuItemProduct menuItemProduct);
}

