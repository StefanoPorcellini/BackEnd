using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;
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
        // sto cercando di implementara una pagina di Login all'inizio, ma non ancora riesco
        public IActionResult Index()
        {
            // Se l'utente è autenticato, visualizza la home page
        //    if (User.Identity.IsAuthenticated)
        //    {
                return View();
        //    }
        //    else
        //    {
        //        // Altrimenti, reindirizza alla pagina di login
        //        return RedirectToAction("Login");
        //    }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Login()
        //{
        //    // Visualizza la vista di login
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // Invia la richiesta di login all'API
        //    var httpClient = new HttpClient();
        //    var response = await httpClient.PostAsJsonAsync("api/auth", model);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
        //        // Salva il token nel session storage del client (via JavaScript)
        //        TempData["AuthToken"] = result?.Token;
        //        TempData["UserId"] = result?.UserId;

        //        // Reindirizza alla home page
        //        return RedirectToAction("Index");
        //    }

        //    // Mostra un messaggio di errore se il login fallisce
        //    ModelState.AddModelError(string.Empty, "Login failed");
        //    return View(model);
        //}



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

