using Esercizio_M5_W1_D5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_M5_W1_D5.Controllers
{
    public class VerbaliController : Controller
    {
        private readonly IVerbaleService _verbaleService;
        public VerbaliController(IVerbaleService verbaleService)
        {
            _verbaleService = verbaleService;
        }
        //Verbali Get All
        public async Task<IActionResult> Index()
        {
            var verbali = await Task.Run(() => _verbaleService.GetAllVerbali()); 
            return View(verbali);
        }

        //dettaglio verbale 
        public async Task<IActionResult> Details(int id)
        {
            var verbale = await Task.Run(() => _verbaleService.GetById(id));
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }
            
        // Violazioni con decurtamento punti > 10
        public async Task<IActionResult> DieciPunti()
        {
            var violazioni = await Task.Run(() => _verbaleService.GetViolazioniOltre10Punti());
            return View(violazioni);
        }

        // violazioni il cui importo è maggiore di 400 euro
        public async Task<IActionResult> MultaSalata()
        {
            var violazioni = await Task.Run(() => _verbaleService.GetViolazioniOltre400Euro());
            return View(violazioni);
        }

    }
}
