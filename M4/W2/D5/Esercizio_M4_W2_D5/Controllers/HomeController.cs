using Esercizio_M4_W2_D5.Models;
using Esercizio_M4_W2_D5.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Esercizio_M4_W2_D5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
            
        }

        public IActionResult Index()
        {
            var product = _productService.GetProducts();
            List<ProductViewModel> productViewModels = product.Select(p=> new ProductViewModel 
            {
                Id = p.Id,
                Brand = p.Brand,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImgCover = p.ImgCover,
            }).ToList();

            return View(productViewModels);
        }

        public IActionResult Details(int id)
        {
            var products = _productService.GetProductById(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
