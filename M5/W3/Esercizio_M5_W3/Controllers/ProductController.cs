using Esercizio_Pizzeria_In_Forno.Models;
using Microsoft.AspNetCore.Mvc;
using Esercizio_Pizzeria_In_Forno.Service;

namespace Esercizio_Pizzeria_In_Forno.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Pagina Index con tutti i prodotti
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }


        // Crea un nuovo prodotto
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                return RedirectToAction("ProductDetails", new { id = createdProduct.Id });
            }
            return View(product);
        }

        // Dettagli di un prodotto specifico
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Ottieni tutti i prodotti
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // Modifica un prodotto
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            if (ModelState.IsValid)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                // Aggiorna le proprietà necessarie
                product.Name = updatedProduct.Name;
                product.Photo = updatedProduct.Photo;
                product.Price = updatedProduct.Price;
                product.DeliveryTimeInMinutes = updatedProduct.DeliveryTimeInMinutes;
                product.Ingredients = updatedProduct.Ingredients;

                var updatedProductResult = await _productService.UpdateProductAsync(product);
                return RedirectToAction("ProductDetails", new { id = updatedProductResult.Id });
            }
            return View(updatedProduct);
        }

        // Elimina un prodotto
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("GetAllProducts");
        }
    }
}
