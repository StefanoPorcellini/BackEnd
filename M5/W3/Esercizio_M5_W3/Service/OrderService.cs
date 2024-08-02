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

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order, List<ProductToOrder> products, int userId)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var product in products)
            {
                product.IdOrder = order.Id;
                _context.ProductToOrders.Add(product);
            }

            // Associa l'ordine all'utente
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.OrderId = order.Id;
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return order;
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

            var existingOrder = await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == order.Id);
            if (existingOrder == null) throw new KeyNotFoundException("Order not found");

            existingOrder.Address = order.Address;
            existingOrder.Note = order.Note;
            existingOrder.Processed = order.Processed;

            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("Invalid order ID", nameof(orderId));

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) throw new KeyNotFoundException("Order not found");

            // Rimuovi l'associazione tra l'ordine e l'utente
            var usersWithOrder = await _context.Users.Where(u => u.OrderId == orderId).ToListAsync();
            foreach (var user in usersWithOrder)
            {
                user.OrderId = null;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task AddToOrderAsync(int userId, int productId)
        {
            // Trova l'utente
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("Invalid user ID");

            // Trova l'ordine esistente dell'utente o crea un nuovo ordine
            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Users.Any(u => u.Id == userId) && !o.Processed);

            if (order == null)
            {
                order = new Order
                {
                    Address = "Default Address",
                    Note = "Default Note",
                    Users = new List<User> { user }
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Salva l'ordine per ottenere l'ID
            }

            // Trova il prodotto
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ArgumentException("Invalid product ID");

            // Verifica se il prodotto è già stato aggiunto all'ordine
            var existingProductToOrder = await _context.ProductToOrders
                .FirstOrDefaultAsync(p => p.IdOrder == order.Id && p.IdProduct == productId);

            if (existingProductToOrder != null)
            {
                // Aggiorna la quantità se il prodotto è già presente
                existingProductToOrder.Quantity += 1;
                _context.ProductToOrders.Update(existingProductToOrder);
            }
            else
            {
                // Aggiungi un nuovo record se il prodotto non è presente
                var productToOrder = new ProductToOrder
                {
                    IdOrder = order.Id,
                    IdProduct = product.Id,
                    Quantity = 1
                };
                _context.ProductToOrders.Add(productToOrder);
            }

            await _context.SaveChangesAsync();
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

    }
}
