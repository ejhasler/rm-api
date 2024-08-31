using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            return NoContent();
        }
    }
}
