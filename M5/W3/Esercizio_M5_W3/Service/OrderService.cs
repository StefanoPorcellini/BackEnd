using Esercizio_Pizzeria_In_Forno.Models;
using Esercizio_Pizzeria_In_Forno.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esercizio_Pizzeria_In_Forno.Models.ViewModels;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ILogger<OrderService> _logger;


        public OrderService(DataContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Order> CreateOrderAsync(Order order, List<ProductToOrder> products, int userId)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var product in products)
                {
                    product.IdOrder = order.Id;
                    _context.ProductToOrders.Add(product);
                }

                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.OrderId = order.Id;
                    _context.Users.Update(user);
                }

                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione dell'ordine.");
                throw;
            }
        }


        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("Invalid order ID", nameof(orderId));

            return await _context.Orders
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .Where(o => o.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Products)
                .ThenInclude(p => p.Product)
                .ToListAsync();
        }
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            try
            {
                var existingOrder = await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == order.Id);
                if (existingOrder == null) throw new KeyNotFoundException("Order not found");

                existingOrder.Address = order.Address;
                existingOrder.Note = order.Note;
                existingOrder.Processed = order.Processed;

                await _context.SaveChangesAsync();
                return existingOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento dell'ordine.");
                throw;
            }
        }
        public async Task DeleteOrderAsync(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("Invalid order ID", nameof(orderId));

            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null) throw new KeyNotFoundException("Order not found");

                var usersWithOrder = await _context.Users.Where(u => u.OrderId == orderId).ToListAsync();
                foreach (var user in usersWithOrder)
                {
                    user.OrderId = null;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione dell'ordine.");
                throw;
            }
        }

        public async Task AddToOrderAsync(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("Utente non trovato");
                }

                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == user.OrderId && !o.Processed);
                if (order == null)
                {
                    throw new InvalidOperationException("Ordine non trovato o già processato");
                }

                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException("Prodotto non trovato");
                }

                var productToOrder = new ProductToOrder
                {
                    IdProduct = productId,
                    IdOrder = order.Id,
                    Quantity = 1 // impostiamo una quantità predefinita
                };

                _context.ProductToOrders.Add(productToOrder);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiunta del prodotto all'ordine.");
                throw;
            }
        }


        public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                .ThenInclude(po => po.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            var orderDetails = new OrderDetailsViewModel
            {
                OrderId = order.Id,
                Products = order.Products.Select(po => new ProductDetailsViewModel
                {
                    ProductId = po.Product.Id,
                    Name = po.Product.Name,
                    Quantity = po.Quantity,
                    Price = po.Product.Price,
                    Total = po.Quantity * po.Product.Price
                }).ToList()
            };

            orderDetails.TotalPrice = orderDetails.Products.Sum(p => p.Total);

            return orderDetails;
        }
        public async Task UpdateProductQuantityAsync(int orderId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("La quantità deve essere maggiore di zero.");
            }

            try
            {
                var productToOrder = await _context.ProductToOrders
                    .FirstOrDefaultAsync(p => p.IdProduct == productId && p.IdOrder == orderId);

                if (productToOrder != null)
                {
                    productToOrder.Quantity = quantity;
                    _context.ProductToOrders.Update(productToOrder);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Prodotto non trovato nell'ordine");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento della quantità del prodotto.");
                throw;
            }
        }

        public async Task DeleteProductFromOrderAsync(int orderId, int productId)
        {
            try
            {
                var productToOrder = await _context.ProductToOrders
                    .FirstOrDefaultAsync(p => p.IdProduct == productId && p.IdOrder == orderId);

                if (productToOrder != null)
                {
                    _context.ProductToOrders.Remove(productToOrder);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Prodotto non trovato nell'ordine");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione del prodotto dall'ordine.");
                throw;
            }
        }
    }
}