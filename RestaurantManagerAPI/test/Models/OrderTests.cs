using System.ComponentModel.DataAnnotations;
using RestaurantManagerAPI.Models;
using FluentAssertions;

namespace RestaurantManagerAPI.Tests.Models
{
    public class OrderTests
    {
        private List<ValidationResult> ValidateModel(Order order)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(order, null, null);
            Validator.TryValidateObject(order, context, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void Order_DateTimeIsDefault_ShouldHaveValidationError()
        {
            // Arrange
            var order = new Order
            {
                DateTime = default, // Default DateTime value
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem() }
            };

            // Act
            var results = ValidateModel(order);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("DateTime") && r.ErrorMessage.Contains("default value"));
        }

        [Fact]
        public void Order_DateTimeIsValid_ShouldPassValidation()
        {
            // Arrange
            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem() }
            };

            // Act
            var results = ValidateModel(order);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void Order_OrderMenuItemsIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = null // Null OrderMenuItems list
            };

            // Act
            var results = ValidateModel(order);

            // Assert
            results.Should().Contain(r => r.MemberNames.Contains("OrderMenuItems") && r.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Order_OrderMenuItemsIsEmpty_ShouldPassValidation()
        {
            // Arrange
            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem>() // Empty list should pass validation
            };

            // Act
            var results = ValidateModel(order);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void Order_ValidOrder_ShouldPassValidation()
        {
            // Arrange
            var order = new Order
            {
                DateTime = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem> { new OrderMenuItem() }
            };

            // Act
            var results = ValidateModel(order);

            // Assert
            results.Should().BeEmpty();
        }
    }
}
