using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace RestaurantManagerAPI.Services
{
    /// <summary>
    /// Service for managing menu items in the restaurant application.
    /// </summary>
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemService"/> class.
        /// </summary>
        /// <param name="menuItemRepository">The repository for accessing menu item data.</param>
        /// <param name="productService">The service for accessing product data.</param>
        public MenuItemService(IMenuItemRepository menuItemRepository, IProductService productService)
        {
            _menuItemRepository = menuItemRepository;
            _productService = productService;
        }

        /// <summary>
        /// Retrieves all menu items asynchronously.
        /// </summary>
        /// <returns>A list of all menu items.</returns>
        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _menuItemRepository.GetAllMenuItemsAsync();
        }

        /// <summary>
        /// Retrieves a menu item by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the menu item.</param>
        /// <returns>The menu item with the specified ID.</returns>
        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            return await _menuItemRepository.GetMenuItemByIdAsync(id);
        }

        /// <summary>
        /// Adds a new menu item asynchronously.
        /// </summary>
        /// <param name="menuItem">The menu item to add.</param>
        /// <returns>The newly added menu item.</returns>
        /// <exception cref="ArgumentException">Thrown when the menu item is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when a product is not found.</exception>
        public async Task<MenuItem> AddMenuItemAsync(MenuItem menuItem)
        {
            ValidateMenuItem(menuItem);

            // Validate and add menu item products
            foreach (var productId in menuItem.ProductIds)
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {productId} does not exist.");
                }

                menuItem.MenuItemProducts.Add(new MenuItemProduct { ProductId = productId });
            }

            return await _menuItemRepository.AddMenuItemAsync(menuItem);
        }

        /// <summary>
        /// Updates an existing menu item asynchronously.
        /// </summary>
        /// <param name="menuItem">The menu item with updated details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the menu item is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when a menu item or product is not found.</exception>
        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            ValidateMenuItem(menuItem);

            var existingMenuItem = await _menuItemRepository.GetMenuItemByIdAsync(menuItem.Id);
            if (existingMenuItem == null)
            {
                throw new KeyNotFoundException($"MenuItem with ID {menuItem.Id} does not exist.");
            }

            existingMenuItem.Name = menuItem.Name;
            existingMenuItem.MenuItemProducts.Clear();

            // Update the products associated with the menu item
            foreach (var productId in menuItem.ProductIds)
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {productId} does not exist.");
                }

                existingMenuItem.MenuItemProducts.Add(new MenuItemProduct { ProductId = productId });
            }

            await _menuItemRepository.UpdateMenuItemAsync(existingMenuItem);
        }

        /// <summary>
        /// Deletes a menu item by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the menu item to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteMenuItemAsync(int id)
        {
            await _menuItemRepository.DeleteMenuItemAsync(id);
        }

        /// <summary>
        /// Validates the menu item using the built-in validation attributes.
        /// </summary>
        /// <param name="menuItem">The menu item to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the menu item is invalid.</exception>
        private void ValidateMenuItem(MenuItem menuItem)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(menuItem, null, null);
            if (!Validator.TryValidateObject(menuItem, context, validationResults, true))
            {
                var validationMessage = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException(validationMessage);
            }
        }
    }
}
