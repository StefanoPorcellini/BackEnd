using Esercizio_M4_W2_D3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Esercizio_M4_W2_D3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = new CinemaViewModel
            {
                Sale = CinemaData.Sale,
                Ospite = new Ospite()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult VendiBiglietto(CinemaViewModel viewModel)
        {
            var selectedSala = CinemaData.Sale.FirstOrDefault(s => s.Nome == viewModel.Ospite.Sala);
            if (selectedSala != null && selectedSala.BigliettiVenduti < selectedSala.CapienzaMassima)
            {
                CinemaData.Ospiti.Add(viewModel.Ospite);
                selectedSala.BigliettiVenduti++;
                if (viewModel.Ospite.TipoBiglietto == TipoBiglietto.Ridotto)
                {
                    selectedSala.BigliettiRidotti++;
                }
            }

            return RedirectToAction("Biglietti");
        }

        public IActionResult Biglietti()
        {
            var viewModel = new CinemaViewModel
            {
                Sale = CinemaData.Sale,
                Ospite = new Ospite()
            };
            return View(viewModel);
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
