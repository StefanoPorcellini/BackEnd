using Esercizio_M5_W1_D5.Models;
using Esercizio_M5_W1_D5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_M5_W1_D5.Controllers
{
    public class AnagrafeController : Controller
    {
        private readonly IAnagraficaService _anagraficaService;
        public AnagrafeController(IAnagraficaService anagraficaservice)
        {
            _anagraficaService = anagraficaservice;
        }
        //Anagrafica GetAll
        public async Task<IActionResult> Index()
        {
            var trasgressori = await Task.Run(() => _anagraficaService.GetAllTrasgressore());
            return View(trasgressori);
        }
        //Anagrafica Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cognome,Nome,Indirizzo,Città,CAP,CF")] Anagrafica trasgressore)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => _anagraficaService.AddTrasgressore(trasgressore));
                return RedirectToAction(nameof(Index));
            }
            return View(trasgressore);
        }

    }
}
