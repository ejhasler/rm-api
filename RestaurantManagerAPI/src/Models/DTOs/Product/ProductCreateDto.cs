using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.DTOs;

/// <summary>
/// Represents a Data Transfer Object (DTO) for creating a
/// <see cref="Product"/>.
/// </summary>
/// <author>Even Johan Pereira Haslerud</author>
/// <date>30.08.2021</date>
public class ProductCreateDto
{

    /// <summary>
    /// Gets or sets the name of the Product. The name is
    /// a required field and should not contain any numeric values.
    /// </summary>
    /// <value>
    /// A string representing the name of the Product, which must
    /// be a valid string containing only alphabetical characters and spaces.
    /// </value>
    /// <example>Chicken</example>
    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain numbers.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the portion count of the Product. The portion count is
    /// a required field and must be greater than 0.
    /// </summary>
    /// <value>
    /// A double representing the portion count of the Product.
    /// </value>
    /// <example>1.0</example>
    [Range(0.1, double.MaxValue, ErrorMessage = "Portion count must be greater than 0.")]
    public double PortionCount { get; set; }

    /// <summary>
    /// Gets or sets the unit of the Product. The unit is
    /// a required field.
    /// </summary>
    /// <value>
    /// A string representing the unit of the Product.
    /// </value>
    /// <example>kg</example>
    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; }

    /// <summary>
    /// Gets or sets the portion size of the Product. The portion size is
    /// a required field and must be greater than 0.
    /// </summary>
    /// <value>
    /// A double representing the portion size of the Product.
    /// </value>
    /// <example>0.5</example>
    [Range(0.1, double.MaxValue, ErrorMessage = "Portion size must be greater than 0.")]
    public double PortionSize { get; set; }
}
