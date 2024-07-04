using Esercizio_M3_W3_D4.Models;
using Esercizio_M3_W3_D4.Serices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Esercizio_M3_W3_D4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly I_Impiegato _impiegato;

        public HomeController(ILogger<HomeController> logger, I_Impiegato impiegato)
        {
            _logger = logger;
            _impiegato = impiegato;
        }

        public IActionResult Index()
        {
            return View(_impiegato.GetAll());
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
