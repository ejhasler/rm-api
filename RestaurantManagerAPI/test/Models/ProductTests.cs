using System.ComponentModel.DataAnnotations;
using RestaurantManagerAPI.Models;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Models
{
    public class ProductTests
    {
        private List<ValidationResult> ValidateModel(Product product)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            Validator.TryValidateObject(product, context, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void Product_NameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = null,
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Product_NameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = string.Empty,
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Product_UnitIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = null,
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Unit") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Product_UnitIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = string.Empty,
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Unit") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Product_PortionCountIsLessThanOrEqualToZero_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chicken",
                PortionCount = 0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("PortionCount") && r.ErrorMessage.Contains("greater than 0"));
        }

        [Fact]
        public void Product_PortionSizeIsLessThanOrEqualToZero_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("PortionSize") && r.ErrorMessage.Contains("greater than 0"));
        }

        [Fact]
        public void Product_NameContainsNumbers_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chicken123",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("cannot contain numbers"));
        }

        [Fact]
        public void Product_NameContainsSpecialCharacters_ShouldHaveValidationError()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chick#n",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("cannot contain numbers"));
        }

        [Fact]
        public void Product_ValidProduct_ShouldPassValidation()
        {
            // Arrange
            var product = new Product
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            results.Should().BeEmpty();
        }
    }
}
