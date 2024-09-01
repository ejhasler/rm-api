using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.DTOs;

/// <summary>
/// Represents a Data Transfer Object (DTO) for creating a
/// <see cref="MenuItem"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class MenuItemCreateDto
{
    /// <summary>
    /// Gets or sets the name of the Menu Item. The name is
    /// a required field and should not contain any numeric values.
    /// </summary>
    /// <value>
    /// A string representing the name of the Menu Item, which must
    /// be a valid string containing only alphabetical characters and spaces.
    /// </value>
    /// <example>Grilled Chicken Salad</example>
    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain numbers.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of product IDs associated with this Menu Item.
    /// This list represents the products required to prepare this Menu Item.
    /// </summary>
    /// <value>
    /// A list of integers, where each integer is the unique identifier of a
    /// product that is part of the menu item.
    /// </value>
    /// <example>
    /// [1, 2, 3]
    /// </example>
    public List<int> ProductIds { get; set; } = new List<int>();
}
