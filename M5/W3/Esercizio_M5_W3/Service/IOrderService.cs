using Esercizio_Pizzeria_In_Forno.Models;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order, List<ProductToOrder> products, int userId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task AddToOrderAsync(int userId, int productId);
    }
}
