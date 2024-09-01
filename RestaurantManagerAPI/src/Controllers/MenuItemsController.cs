using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.DTOs;

namespace RestaurantManagerAPI.Controllers
{
    /// <summary>
    /// Controller for managing menu items in the restaurant.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemsController"/> class.
        /// </summary>
        /// <param name="menuItemService">The service used to manage menu items.</param>
        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        /// <summary>
        /// Gets all menu items.
        /// </summary>
        /// <returns>A list of all menu items.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemReadDto>>> GetAllMenuItems()
        {
            var menuItems = await _menuItemService.GetAllMenuItemsAsync();
            var menuItemsReadDto = menuItems.Select(mi => new MenuItemReadDto
            {
                Id = mi.Id,
                Name = mi.Name,
                ProductIds = mi.MenuItemProducts.Select(mp => mp.ProductId).ToList()
            });

            return Ok(menuItemsReadDto);
        }

        /// <summary>
        /// Gets a specific menu item by ID.
        /// </summary>
        /// <param name="id">The ID of the menu item.</param>
        /// <returns>The menu item with the specified ID, or a 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemReadDto>> GetMenuItem(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            var menuItemReadDto = new MenuItemReadDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                ProductIds = menuItem.MenuItemProducts.Select(mp => mp.ProductId).ToList()
            };

            return Ok(menuItemReadDto);
        }

        /// <summary>
        /// Adds a new menu item.
        /// </summary>
        /// <param name="menuItemCreateDto">The details of the menu item to add.</param>
        /// <returns>The newly created menu item.</returns>
        [HttpPost]
        public async Task<ActionResult<MenuItemReadDto>> AddMenuItem([FromBody] MenuItemCreateDto menuItemCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuItem = new MenuItem
            {
                Name = menuItemCreateDto.Name,
                ProductIds = menuItemCreateDto.ProductIds
            };

            var newMenuItem = await _menuItemService.AddMenuItemAsync(menuItem);

            var menuItemReadDto = new MenuItemReadDto
            {
                Id = newMenuItem.Id,
                Name = newMenuItem.Name,
                ProductIds = newMenuItem.MenuItemProducts.Select(mp => mp.ProductId).ToList()
            };

            return CreatedAtAction(nameof(GetMenuItem), new { id = menuItemReadDto.Id }, menuItemReadDto);
        }

        /// <summary>
        /// Updates an existing menu item.
        /// </summary>
        /// <param name="id">The ID of the menu item to update.</param>
        /// <param name="menuItemUpdateDto">The updated details of the menu item.</param>
        /// <returns>A 200 OK if the update is successful, or a 400 if the IDs do not match.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemUpdateDto menuItemUpdateDto)
        {
            if (id != menuItemUpdateDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var menuItem = new MenuItem
            {
                Id = menuItemUpdateDto.Id,
                Name = menuItemUpdateDto.Name,
                ProductIds = menuItemUpdateDto.ProductIds
            };

            await _menuItemService.UpdateMenuItemAsync(menuItem);

            return Ok(menuItem);
        }

        /// <summary>
        /// Deletes a menu item by ID.
        /// </summary>
        /// <param name="id">The ID of the menu item to delete.</param>
        /// <returns>A 204 No Content if the deletion is successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            return NoContent();
        }
    }
}
