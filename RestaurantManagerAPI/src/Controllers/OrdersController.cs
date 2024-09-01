using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.DTOs;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Services;

namespace RestaurantManagerAPI.Controllers
{
    /// <summary>
    /// Controller for managing orders in the restaurant stock.
    /// </summary>
    /// <author>Even Johan Pereira Haslerud</author>
    /// <date>30.08.2024</date>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderService">Service for handling order operations.</param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
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

        /// <summary>
        /// Gets a specific order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>The order with the specified ID, or a 404 if not found.</returns>
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

        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="orderCreateDto">The order to add.</param>
        /// <returns>The newly created order.</returns>
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

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="orderUpdateDto">The updated order data.</param>
        /// <returns>A 200 OK if successful, 400 if there is an ID mismatch, or 404 if the order is not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
        {
            if (id != orderUpdateDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound("Order not found.");
            }

            existingOrder.DateTime = orderUpdateDto.DateTime;
            existingOrder.OrderMenuItems = orderUpdateDto.MenuItemIds.Select(id => new OrderMenuItem { MenuItemId = id }).ToList();

            var updatedOrder = await _orderService.UpdateOrderAsync(existingOrder);

            if (updatedOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the order.");
            }

            var orderReadDto = new OrderReadDto
            {
                Id = updatedOrder.Id,
                DateTime = updatedOrder.DateTime,
                MenuItemIds = updatedOrder.OrderMenuItems.Select(omi => omi.MenuItemId).ToList()
            };

            return Ok(orderReadDto);
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A 204 No Content response if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}

