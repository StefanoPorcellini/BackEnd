using Esercizio_Pizzeria_In_Forno.Models;
using Esercizio_Pizzeria_In_Forno.Service;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_Pizzeria_In_Forno.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //crea nuovo ordine
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var createOrder = await _orderService.CreateOrderAsync(order);
                return RedirectToAction("OrderDetails", new { id = createOrder.Id })
            }
            return View(order);
        }

        // Dettagli di un ordine specifico
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            { 
                return NotFound();
            }
            return View(order);
        }

        // Ottieni tutti gli ordini
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GettAllOrderAsync();
            return View(orders);
        }

        // Ottieni ordini di un utente specifico
        [HttpGet]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrderByUserIdAsync(userId);
            return View(orders);
        }

        // Aggiorna un ordine
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(order);
                return RedirectToAction("OrderDetails", new { id = updatedOrder.Id });
            }
            return View(order);
        }

        // Elimina un ordine
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("GetAllOrders");
        }

    }
}
