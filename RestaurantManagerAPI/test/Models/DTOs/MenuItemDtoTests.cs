using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using RestaurantManagerAPI.DTOs;

namespace RestaurantManagerAPI.Tests.DTOs
{
    public class MenuItemDtoTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            return validationResults;
        }

        #region MenuItemCreateDto Tests

        [Fact]
        public void MenuItemCreateDto_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            // Arrange
            var dto = new MenuItemCreateDto
            {
                Name = "Grilled Chicken Salad",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void MenuItemCreateDto_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new MenuItemCreateDto
            {
                Name = string.Empty,
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name is required.");
        }

        [Fact]
        public void MenuItemCreateDto_ShouldBeInvalid_WhenNameContainsNumbers()
        {
            // Arrange
            var dto = new MenuItemCreateDto
            {
                Name = "Grilled Chicken 1",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name cannot contain numbers.");
        }

        #endregion

        #region MenuItemReadDto Tests

        [Fact]
        public void MenuItemReadDto_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            // Arrange
            var dto = new MenuItemReadDto
            {
                Id = 1,
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void MenuItemReadDto_ShouldBeInvalid_WhenIdIsZeroOrLess()
        {
            // Arrange
            var dto = new MenuItemReadDto
            {
                Id = 0,
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Id must be greater than 0.");
        }

        [Fact]
        public void MenuItemReadDto_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new MenuItemReadDto
            {
                Id = 1,
                Name = string.Empty,
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name is required.");
        }

        [Fact]
        public void MenuItemReadDto_ShouldBeInvalid_WhenNameContainsNumbers()
        {
            // Arrange
            var dto = new MenuItemReadDto
            {
                Id = 1,
                Name = "Grilled Chicken 1",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name cannot contain numbers.");
        }

        #endregion

        #region MenuItemUpdateDto Tests

        [Fact]
        public void MenuItemUpdateDto_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            // Arrange
            var dto = new MenuItemUpdateDto
            {
                Id = 1,
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void MenuItemUpdateDto_ShouldBeInvalid_WhenIdIsZeroOrLess()
        {
            // Arrange
            var dto = new MenuItemUpdateDto
            {
                Id = 0,
                Name = "Grilled Chicken Sandwich",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Id must be greater than 0.");
        }

        [Fact]
        public void MenuItemUpdateDto_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new MenuItemUpdateDto
            {
                Id = 1,
                Name = string.Empty,
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name is required.");
        }

        [Fact]
        public void MenuItemUpdateDto_ShouldBeInvalid_WhenNameContainsNumbers()
        {
            // Arrange
            var dto = new MenuItemUpdateDto
            {
                Id = 1,
                Name = "Grilled Chicken 1",
                ProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name cannot contain numbers.");
        }

        #endregion
    }
}
