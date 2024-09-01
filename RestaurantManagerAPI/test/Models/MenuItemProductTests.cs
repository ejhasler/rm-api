using FluentAssertions;
using RestaurantManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerAPI.Tests.Models
{
    public class MenuItemProductTests
    {
        private readonly MenuItemProduct _menuItemProduct;

        public MenuItemProductTests()
        {
            _menuItemProduct = new MenuItemProduct();
        }

        private List<ValidationResult> ValidateModel(MenuItemProduct model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        #region Required Fields

        [Fact]
        public void MenuItemProduct_MissingMenuItemId_ShouldHaveValidationError()
        {
            // Arrange
            _menuItemProduct.MenuItemId = 0; // Invalid value (default for int, triggers Range validation)
            _menuItemProduct.ProductId = 5; // Valid value

            // Act
            var validationResults = ValidateModel(_menuItemProduct);

            // Assert
            validationResults.Should().ContainSingle(result =>
                result.ErrorMessage == "MenuItemId must be greater than 0.");
        }

        [Fact]
        public void MenuItemProduct_MissingProductId_ShouldHaveValidationError()
        {
            // Arrange
            _menuItemProduct.MenuItemId = 1; // Valid value
            _menuItemProduct.ProductId = 0; // Invalid value (default for int, triggers Range validation)

            // Act
            var validationResults = ValidateModel(_menuItemProduct);

            // Assert
            validationResults.Should().ContainSingle(result =>
                result.ErrorMessage == "ProductId must be greater than 0.");
        }


        [Fact]
        public void MenuItemProduct_AllFieldsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            _menuItemProduct.MenuItemId = 1; // Valid value
            _menuItemProduct.ProductId = 5; // Valid value

            // Act
            var validationResults = ValidateModel(_menuItemProduct);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void MenuItemProduct_InvalidMenuItemAndProduct_ShouldHaveValidationErrors()
        {
            // Arrange
            _menuItemProduct.MenuItemId = 0; // Invalid value
            _menuItemProduct.ProductId = 0; // Invalid value

            // Act
            var validationResults = ValidateModel(_menuItemProduct);

            // Assert
            validationResults.Should().HaveCount(2);
            validationResults.Should().Contain(result =>
                result.ErrorMessage == "MenuItemId must be greater than 0.");
            validationResults.Should().Contain(result =>
                result.ErrorMessage == "ProductId must be greater than 0.");
        }

        #endregion
    }
}
