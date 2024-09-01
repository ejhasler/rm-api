using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using RestaurantManagerAPI.DTOs;

namespace RestaurantManagerAPI.Tests.DTOs
{
    public class OrderDtoTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            return validationResults;
        }

        #region OrderCreateDto Tests

        [Fact]
        public void OrderCreateDto_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            var dto = new OrderCreateDto
            {
                DateTime = DateTime.Now,
                MenuItemIds = new List<int> { 1, 2, 3 }
            };

            var validationResults = ValidateModel(dto);
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderCreateDto_ShouldBeInvalid_WhenDateTimeIsDefault()
        {
            var dto = new OrderCreateDto
            {
                DateTime = DateTime.MinValue, // Default value that should fail validation
                MenuItemIds = new List<int> { 1, 2, 3 }
            };

            var validationResults = ValidateModel(dto);

            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("DateTime must be a valid date and cannot be DateTime.MinValue.");
        }

        #endregion

        #region OrderReadDto Tests

        [Fact]
        public void OrderReadDto_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            var dto = new OrderReadDto
            {
                Id = 1,
                DateTime = DateTime.Now,
                MenuItemIds = new List<int> { 1, 2, 3 }
            };

            var validationResults = ValidateModel(dto);
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderReadDto_ShouldBeInvalid_WhenDateTimeIsDefault()
        {
            var dto = new OrderReadDto
            {
                Id = 1,
                DateTime = DateTime.MinValue, // Default value that should fail validation
                MenuItemIds = new List<int> { 1, 2, 3 }
            };

            var validationResults = ValidateModel(dto);

            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("DateTime must be a valid date and cannot be DateTime.MinValue.");
        }

        #endregion

        #region OrderUpdateDto Tests

        [Fact]
        public void OrderUpdateDto_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            var dto = new OrderUpdateDto
            {
                Id = 1,
                DateTime = DateTime.Now,
                MenuItemIds = new List<int> { 1, 2, 3 }
            };

            var validationResults = ValidateModel(dto);
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void OrderUpdateDto_ShouldBeInvalid_WhenDateTimeIsDefault()
        {
            var dto = new OrderUpdateDto
            {
                Id = 1,
                DateTime = DateTime.MinValue, // Default value that should fail validation
                MenuItemIds = new List<int> { 1, 2, 3 }
            };

            var validationResults = ValidateModel(dto);

            validationResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("DateTime must be a valid date and cannot be DateTime.MinValue.");
        }

        #endregion
    }
}
