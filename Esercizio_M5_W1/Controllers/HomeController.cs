using Esercizio_M5_W1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Esercizio_M5_W1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            IEnumerable<Spedizione> spedizioni = null;

            if (TempData["Spedizioni"] != null)
            {
                spedizioni = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Spedizione>>(TempData["Spedizioni"].ToString());
            }

            return View(spedizioni);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
