using RestaurantManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> AddOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
