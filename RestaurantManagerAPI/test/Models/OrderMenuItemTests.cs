using FluentAssertions;
using RestaurantManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.Tests.Models
{
    public class OrderMenuItemTests
    {
        private readonly OrderMenuItem _orderMenuItem;

        public OrderMenuItemTests()
        {
            _orderMenuItem = new OrderMenuItem();
        }

        private List<ValidationResult> ValidateModel(OrderMenuItem model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        #region Required Fields

        [Fact]
        public void OrderMenuItem_MissingOrderId_ShouldHaveValidationError()
        {
            // Arrange
            _orderMenuItem.OrderId = 0; // Invalid value (default for int, triggers Range validation)
            _orderMenuItem.MenuItemId = 3; // Valid value
            _orderMenuItem.Order = new Order(); // Valid associated object
            _orderMenuItem.MenuItem = new MenuItem(); // Valid associated object

            // Act
            var validationResults = ValidateModel(_orderMenuItem);

            // Assert
            validationResults.Should().ContainSingle(result =>
                result.ErrorMessage == "OrderId must be greater than 0.");
        }

        [Fact]
        public void OrderMenuItem_MissingMenuItemId_ShouldHaveValidationError()
        {
            // Arrange
            _orderMenuItem.OrderId = 1; // Valid value
            _orderMenuItem.MenuItemId = 0; // Invalid value (default for int, triggers Range validation)
            _orderMenuItem.Order = new Order(); // Valid associated object
            _orderMenuItem.MenuItem = new MenuItem(); // Valid associated object

            // Act
            var validationResults = ValidateModel(_orderMenuItem);

            // Assert
            validationResults.Should().ContainSingle(result =>
                result.ErrorMessage == "MenuItemId must be greater than 0.");
        }

        [Fact]
        public void OrderMenuItem_MissingOrder_ShouldHaveValidationError()
        {
            // Arrange
            _orderMenuItem.OrderId = 1; // Valid value
            _orderMenuItem.MenuItemId = 3; // Valid value
            _orderMenuItem.Order = null; // Invalid associated object
            _orderMenuItem.MenuItem = new MenuItem(); // Valid associated object

            // Act
            var validationResults = ValidateModel(_orderMenuItem);

            // Assert
            validationResults.Should().ContainSingle(result =>
                result.ErrorMessage == "The Order field is required.");
        }

        [Fact]
        public void OrderMenuItem_MissingMenuItem_ShouldHaveValidationError()
        {
            // Arrange
            _orderMenuItem.OrderId = 1; // Valid value
            _orderMenuItem.MenuItemId = 3; // Valid value
            _orderMenuItem.Order = new Order(); // Valid associated object
            _orderMenuItem.MenuItem = null; // Invalid associated object

            // Act
            var validationResults = ValidateModel(_orderMenuItem);

            // Assert
            validationResults.Should().ContainSingle(result =>
                result.ErrorMessage == "The MenuItem field is required.");
        }

        [Fact]
        public void OrderMenuItem_AllFieldsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            _orderMenuItem.OrderId = 1; // Valid value
            _orderMenuItem.MenuItemId = 3; // Valid value
            _orderMenuItem.Order = new Order { Id = 1, DateTime = DateTime.Now }; // Valid associated object
            _orderMenuItem.MenuItem = new MenuItem { Id = 3, Name = "Pizza" }; // Valid associated object

            // Act
            var validationResults = ValidateModel(_orderMenuItem);

            // Assert
            validationResults.Should().BeEmpty();
        }

        #endregion
    }
}
