using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.DTOs;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var orderDtos = orders.Select(order => new OrderReadDto
            {
                Id = order.Id,
                DateTime = order.DateTime,
                MenuItemIds = order.OrderMenuItems.Select(omi => omi.MenuItemId).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderReadDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderReadDto
            {
                Id = order.Id,
                DateTime = order.DateTime,
                MenuItemIds = order.OrderMenuItems.Select(omi => omi.MenuItemId).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> AddOrder(OrderCreateDto orderCreateDto)
        {
            var order = new Order
            {
                DateTime = orderCreateDto.DateTime,
                OrderMenuItems = orderCreateDto.MenuItemIds.Select(id => new OrderMenuItem { MenuItemId = id }).ToList()
            };

            var newOrder = await _orderService.AddOrderAsync(order);

            var orderReadDto = new OrderReadDto
            {
                Id = newOrder.Id,
                DateTime = newOrder.DateTime,
                MenuItemIds = newOrder.OrderMenuItems.Select(omi => omi.MenuItemId).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = orderReadDto.Id }, orderReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
        {
            if (id != orderUpdateDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var order = new Order
            {
                Id = orderUpdateDto.Id,
                DateTime = orderUpdateDto.DateTime,
                OrderMenuItems = orderUpdateDto.MenuItemIds.Select(id => new OrderMenuItem { MenuItemId = id }).ToList()
            };

            await _orderService.UpdateOrderAsync(order);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}

