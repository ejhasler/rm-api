using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RestaurantManagerAPI.Models
{
    /// <summary>
    /// Represents a <c>Product</c> in the restaurant stock.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>29.08.2024</date>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain numbers.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the product available in stock.
        /// Example: 10 kg.
        /// </summary>
        [Range(0.1, double.MaxValue, ErrorMessage = "Portion count must be greater than 0.")]
        public double PortionCount { get; set; }

        /// <summary>
        /// Gets or sets the unit of the product.
        /// Example: kg, units
        /// </summary>
        [Required(ErrorMessage = "Unit is required.")]
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the portion size of the product.
        /// Example: 0.5 kg per portion.
        /// </summary>
        [Range(0.1, double.MaxValue, ErrorMessage = "Portion size must be greater than 0.")]
        public double PortionSize { get; set; }
    }
}
