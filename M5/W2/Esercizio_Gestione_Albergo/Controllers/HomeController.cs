using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Esercizio_Gestione_Albergo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, IClienteDAO clienteDao)
        {
            _logger = logger;

        }
        public IActionResult Index() 
        {
            return View();
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

