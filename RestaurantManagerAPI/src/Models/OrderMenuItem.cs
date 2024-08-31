using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.Models;

/// <summary>
/// Represents the relationship between an Order and a Menu Item
/// in the restaurant system. This class defines the many-to-many
/// relationship between <see cref="Order"/> and <see cref="MenuItem"/>.
/// </summary>
/// <remarks>
/// Each instance of <c>OrderMenuItem</c> represents a specific
/// menu item that is part of an order. This helps in maintaining the
/// association between orders and the menu items included in them.
/// </remarks>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class OrderMenuItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the order.
    /// </summary>
    /// <value>
    /// An integer representing the ID of the <see cref="Order"/> associated with this <c>OrderMenuItem</c>.
    /// </value>
    /// <example>1</example>
    [Required(ErrorMessage = "OrderId is required.")]
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the order associated with this menu item.
    /// </summary>
    /// <value>
    /// An <see cref="Order"/> object that this <c>OrderMenuItem</c> is associated with.
    /// </value>
    /// <remarks>
    /// This property provides navigation to the associated <see cref="Order"/> entity.
    /// </remarks>
    [Required]
    public Order Order { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the menu item.
    /// </summary>
    /// <value>
    /// An integer representing the ID of the <see cref="MenuItem"/> associated with this <c>OrderMenuItem</c>.
    /// </value>
    /// <example>3</example>
    [Required(ErrorMessage = "MenuItemId is required.")]
    public int MenuItemId { get; set; }

    /// <summary>
    /// Gets or sets the menu item associated with this order.
    /// </summary>
    /// <value>
    /// A <see cref="MenuItem"/> object that this <c>OrderMenuItem</c> is associated with.
    /// </value>
    /// <remarks>
    /// This property provides navigation to the associated <see cref="MenuItem"/> entity.
    /// </remarks>
    [Required]
    public MenuItem MenuItem { get; set; }
}
