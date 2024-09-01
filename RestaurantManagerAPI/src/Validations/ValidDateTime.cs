using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.Validation
{
    /// <summary>
    /// Custom validation attribute to ensure a DateTime value is valid and not set to DateTime.MinValue.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>01.09.2024</date>
    public class ValidDateTimeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates whether the DateTime value is not set to DateTime.MinValue.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A ValidationResult indicating whether validation succeeded or failed.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime == DateTime.MinValue)
                {
                    return new ValidationResult(ErrorMessage ?? "DateTime must be a valid date and cannot be DateTime.MinValue.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
