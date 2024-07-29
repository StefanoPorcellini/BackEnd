using Esercizio_Pizzeria_In_Forno.Models;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GettAllOrderAsync();
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
    }
}
