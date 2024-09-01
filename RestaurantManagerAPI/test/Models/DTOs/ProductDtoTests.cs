using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using RestaurantManagerAPI.DTOs;

namespace RestaurantManagerAPI.Tests.DTOs
{
    public class ProductDtoTests
    {
        #region ProductCreateDto Tests

        [Fact]
        public void ProductCreateDto_ShouldBeValid_WhenAllFieldsAreValid()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void ProductCreateDto_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name is required.");
        }

        [Fact]
        public void ProductCreateDto_ShouldBeInvalid_WhenNameContainsNumbers()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Chicken123",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name cannot contain numbers.");
        }

        [Fact]
        public void ProductCreateDto_ShouldBeInvalid_WhenPortionCountIsZeroOrLess()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Chicken",
                PortionCount = 0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Portion count must be greater than 0.");
        }

        [Fact]
        public void ProductCreateDto_ShouldBeInvalid_WhenUnitIsEmpty()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Unit is required.");
        }

        [Fact]
        public void ProductCreateDto_ShouldBeInvalid_WhenPortionSizeIsZeroOrLess()
        {
            // Arrange
            var dto = new ProductCreateDto
            {
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Portion size must be greater than 0.");
        }

        #endregion

        #region ProductReadDto Tests

        [Fact]
        public void ProductReadDto_ShouldBeValid_WhenAllFieldsAreValid()
        {
            // Arrange
            var dto = new ProductReadDto
            {
                Id = 1,
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void ProductReadDto_ShouldBeInvalid_WhenIdIsZeroOrLess()
        {
            // Arrange
            var dto = new ProductReadDto
            {
                Id = 0,
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Id must be greater than 0.");
        }

        [Fact]
        public void ProductReadDto_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new ProductReadDto
            {
                Id = 1,
                Name = "",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name is required.");
        }

        #endregion

        #region ProductUpdateDto Tests

        [Fact]
        public void ProductUpdateDto_ShouldBeValid_WhenAllFieldsAreValid()
        {
            // Arrange
            var dto = new ProductUpdateDto
            {
                Id = 1,
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void ProductUpdateDto_ShouldBeInvalid_WhenIdIsZeroOrLess()
        {
            // Arrange
            var dto = new ProductUpdateDto
            {
                Id = 0,
                Name = "Chicken",
                PortionCount = 1.0,
                Unit = "kg",
                PortionSize = 0.5
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Id must be greater than 0.");
        }


        #endregion

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
