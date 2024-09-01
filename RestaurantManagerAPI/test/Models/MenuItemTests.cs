using System.ComponentModel.DataAnnotations;
using RestaurantManagerAPI.Models;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Models
{
    public class MenuItemTests
    {
        private List<ValidationResult> ValidateModel(MenuItem menuItem)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(menuItem, null, null);
            Validator.TryValidateObject(menuItem, context, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void MenuItem_NameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = null,
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void MenuItem_NameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = string.Empty,
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void MenuItem_NameContainsNumbers_ShouldHaveValidationError()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = "Grilled Chicken 123",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("cannot contain numbers"));
        }

        [Fact]
        public void MenuItem_NameContainsSpecialCharacters_ShouldHaveValidationError()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = "Grilled Chicken @ Sandwich",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("Name") && r.ErrorMessage.Contains("cannot contain numbers"));
        }

        [Fact]
        public void MenuItem_ValidMenuItem_ShouldPassValidation()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void MenuItem_ProductIdsIsNull_ShouldPassValidation()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = "Grilled Chicken Sandwich",
                ProductIds = null  // Testing if null is acceptable
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void MenuItem_EmptyProductIds_ShouldPassValidation()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int>()  // Empty list
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void MenuItem_ValidMenuItem_WithMenuItemProductsAndOrderMenuItems_ShouldPassValidation()
        {
            // Arrange
            var menuItem = new MenuItem
            {
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int> { 1, 2, 3 },
                MenuItemProducts = new List<MenuItemProduct> { new MenuItemProduct() },  // Valid related entities
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem() }        // Valid related entities
            };

            // Act
            var results = ValidateModel(menuItem);

            // Assert
            results.Should().BeEmpty();
        }
    }
}
