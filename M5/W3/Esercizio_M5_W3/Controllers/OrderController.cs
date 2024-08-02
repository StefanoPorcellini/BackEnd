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
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, IProductService productService, IUserService userService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _logger = logger;
        }

        // Creazione di un ordine
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order, List<ProductToOrder> products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value ?? "0");
                    var createdOrder = await _orderService.CreateOrderAsync(order, products, userId);
                    return RedirectToAction("OrderDetails", new { id = createdOrder.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante la creazione dell'ordine.");
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la creazione dell'ordine.");
                }
            }
            return View(order);
        }

        // Dettagli di un ordine specifico
        [HttpGet]
        public async Task<IActionResult> OrderDetails()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            // Trova l'ordine corrente non elaborato
            var currentOrder = orders.FirstOrDefault(o => !o.Processed);

            if (currentOrder == null)
            {
                // Se non ci sono ordini, puoi visualizzare una vista differente
                // o reindirizzare a un'altra azione
                ViewBag.Message = "Non hai ancora ordinato niente, torna alla home per ordinare.";
                return View("EmptyOrder");
            }

            // Ottieni i dettagli dell'ordine corrente
            var orderDetails = await _orderService.GetOrderDetailsAsync(currentOrder.Id);

            return View(orderDetails);
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



        // Aggiornamento di un ordine
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updatedOrder = await _orderService.UpdateOrderAsync(order);
                    return RedirectToAction("OrderDetails", new { id = updatedOrder.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiornamento dell'ordine.");
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiornamento dell'ordine.");
                }
            }
            return View(order);
        }

        // Eliminazione di un ordine
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return RedirectToAction("GetAllOrders");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione dell'ordine.");
                return RedirectToAction("GetAllOrders");
            }
        }

        // Aggiungi prodotto all'ordine
        [HttpPost]
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
                _logger.LogError(ex, "Errore durante l'aggiunta del prodotto all'ordine.");
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore imprevisto durante l'aggiunta del prodotto all'ordine.");
                return Json(new { success = false, message = "Errore imprevisto. Per favore, riprova più tardi." });
            }
        }

        // Aggiornamento quantità prodotto
        [HttpPost]
        public async Task<IActionResult> UpdateProductQuantity(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Non sei autenticato." });
            }

            if (quantity <= 0)
            {
                return Json(new { success = false, message = "Quantità non valida." });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            var currentOrder = orders.FirstOrDefault(o => !o.Processed);

            if (currentOrder == null)
            {
                return Json(new { success = false, message = "Nessun ordine in corso trovato." });
            }

            try
            {
                await _orderService.UpdateProductQuantityAsync(currentOrder.Id, productId, quantity);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento della quantità del prodotto.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Eliminazione prodotto
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Non sei autenticato." });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            var currentOrder = orders.FirstOrDefault(o => !o.Processed);

            if (currentOrder == null)
            {
                return Json(new { success = false, message = "Nessun ordine in corso trovato." });
            }

            try
            {
                await _orderService.DeleteProductFromOrderAsync(currentOrder.Id, productId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione del prodotto dall'ordine.");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
