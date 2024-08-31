using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.Models;

/// <summary>
/// Represents the relationship between a Menu Item and a
/// Product in the restaurant menu.
/// This class defines the many-to-many relationship between
/// <see cref="MenuItem"/> and <see cref="Product"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class MenuItemProduct
{
    /// <summary>
    /// Gets or sets the unique identifier of the menu item.
    /// </summary>
    /// <value>
    /// An integer representing the ID of the <see cref="MenuItem"/>
    /// associated with this <c>MenuItemProduct</c>.
    /// </value>
    /// <example>1</example>
    [Required(ErrorMessage = "MenuItemId is required.")]
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the menu item associated with this product.
    /// </summary>
    /// <value>
    /// A <see cref="MenuItem"/> object that this <c>MenuItemProduct</c>
    /// is associated with.
    /// </value>
    public MenuItem MenuItem { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    /// <value>
    /// An integer representing the ID of the <see cref="Product"/> associated with this <c>MenuItemProduct</c>.
    /// </value>
    /// <example>5</example>
    [Required(ErrorMessage = "ProductId is required.")]
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product associated with this menu item.
    /// </summary>
    /// <value>
    /// A <see cref="Product"/> object that this <c>MenuItemProduct</c> is associated with.
    /// </value>
    public Product Product { get; set; }
}
