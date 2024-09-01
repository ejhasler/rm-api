using System.ComponentModel.DataAnnotations;
using RestaurantManagerAPI.Validation;

namespace RestaurantManagerAPI.DTOs;

/// <summary>
/// Represents a Data Transfer Object (DTO) for creating a
/// <see cref="Order"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class OrderCreateDto
{

    /// <summary>
    /// Sets the Date Time of the Order. The Date Time is
    /// a required filed and should be a format.
    /// </summary>
    /// <value>
    /// A DateTime representing the Date Time of the Order.
    /// </value>
    /// <example>2021-08-30T12:00:00</example>
    [Required(ErrorMessage = "DateTime is required.")]
    [ValidDateTime(ErrorMessage = "DateTime must be a valid date and cannot be DateTime.MinValue.")]
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Gets the list of the menu item IDs associated with
    /// this Order.
    /// </summary>
    /// <value>
    /// A list of integers, where each integer is the unique
    /// identifier of a menu item that is part of the order.
    /// </value>
    /// <example>
    /// [1, 2, 3]
    /// </example>
    [Required]
    public List<int> MenuItemIds { get; set; }
}