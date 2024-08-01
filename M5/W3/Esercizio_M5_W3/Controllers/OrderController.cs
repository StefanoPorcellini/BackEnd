using Esercizio_Pizzeria_In_Forno.Models;
using Esercizio_Pizzeria_In_Forno.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Esercizio_Pizzeria_In_Forno.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IProductService productService, IUserService userService)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
        }

        //crea nuovo ordine
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order, List<ProductToOrder> products)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value ?? "0");
                var createdOrder = await _orderService.CreateOrderAsync(order, products, userId);
                return RedirectToAction("OrderDetails", new { id = createdOrder.Id });
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
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // Ottieni ordini di un utente specifico
        [HttpGet]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
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

        // Aggiungi un prodotto all'ordine
        [HttpPost]
        public async Task<IActionResult> AddToOrder(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Esegui il login o registrati prima di acquistare." });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _orderService.AddToOrderAsync(userId, productId);
                return Json(new { success = true });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

