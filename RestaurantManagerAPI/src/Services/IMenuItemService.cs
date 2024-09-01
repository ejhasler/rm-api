using RestaurantManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagerAPI.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);
        Task UpdateMenuItemAsync(MenuItem menuItem);
        Task DeleteMenuItemAsync(int id);
    }
}