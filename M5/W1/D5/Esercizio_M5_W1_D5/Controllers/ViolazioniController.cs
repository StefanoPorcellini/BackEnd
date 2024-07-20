using Esercizio_M5_W1_D5.Models;
using Esercizio_M5_W1_D5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_M5_W1_D5.Controllers
{
    public class ViolazioniController : Controller
    {
        private readonly IViolazioneService _violazioneService;
        public ViolazioniController(IViolazioneService violazioneService)
        {
            _violazioneService = violazioneService;
        }

        //Violazioni GetAll
        public async Task<IActionResult> Index()
        {
            var violazioni = await Task.Run(() => _violazioneService.GetAllViolazioni());
            return View(violazioni);
        }

        //Violazioni Cerate
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descrizione,Importo,DecurtamentoPunti")] Violazione violazione)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => _violazioneService.AddViolazione(violazione));
                return RedirectToAction(nameof(Index));
            }
            return View(violazione);
        }

    }
}
