using System.ComponentModel.DataAnnotations;
using RestaurantManagerAPI.Validation;

namespace RestaurantManagerAPI.DTOs;

/// <summary>
/// Represents a Data Transfer Object (DTO) for updating an
/// <see cref="Order"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2024</date>
public class OrderUpdateDto
{

    /// <summary>
    /// Gets or sets the unique identifier of the Order.
    /// </summary>
    /// <value>
    /// An integer representing the unique identifier of the Order.
    /// </value>
    /// <example>1</example>
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
    public int Id { get; set; }

    /// <summary>
    /// Sets the Date Time of the order. The Date Time is
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
    /// identifier of a menu item that is part of the Order.
    /// </value>
    /// <example>
    /// [1, 2, 3]
    /// </example>
    [Required]
    public List<int> MenuItemIds { get; set; }
}
