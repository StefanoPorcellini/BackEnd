using Esercizio_M5_W1_D5.Models;
using Esercizio_M5_W1_D5.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_M5_W1_D5.Controllers
{
    public class VerbaliController : Controller
    {
        private readonly IVerbaleService _verbaleService;
        private readonly IAnagraficaService _anagraficaService;
        private readonly IViolazioneService _violazioneService;
        private readonly ILogger<VerbaliController> _logger;

        public VerbaliController(IVerbaleService verbaleService, IAnagraficaService anagraficaService, IViolazioneService violazioneService, ILogger<VerbaliController> logger)
        {
            _verbaleService = verbaleService;
            _anagraficaService = anagraficaService;
            _violazioneService = violazioneService;
            _logger = logger;
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

        // Metodo create (utilizzo i metodi per prelevare dal database le informazioni da inserire nelle select dei dropdown per inserire il trasgressore e la violazione
        public async Task<IActionResult> Create()
        {
            var trasgressori = await Task.Run(() => _anagraficaService.GetAllTrasgressore());
            var violazioni = await Task.Run(() => _violazioneService.GetAllViolazioni());

            if (trasgressori == null || violazioni == null)
            {
                throw new InvalidOperationException("Trasgressori o Violazioni non possono essere nulli");
            }

            ViewBag.Trasgressori = trasgressori.Select(t => new { t.IDAnagrafica, NomeCompleto = $"{t.Nome} {t.Cognome}" });
            ViewBag.Violazioni = violazioni.Select(v => new { v.IDViolazione, v.Descrizione });


            return View();
        }

        // Metodo create POST ho aggiunto dei logger per la verifica del corretto inserimento dei dati
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataViolazione, IndirizzoViolazione, NominativoAgente, DataTrascrizioneVerbale, Anagrafica_FK, Violazione_FK")] Verbale verbale)
        {
            _logger.LogInformation("Create method called");
            foreach (var key in Request.Form.Keys)
            {
                _logger.LogInformation($"Form key: {key}, Form value: {Request.Form[key]}");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(() => _verbaleService.AddVerbale(verbale));
                    _logger.LogInformation("Verbale added successfully");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding verbale: {ex.Message}");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is not valid");
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        _logger.LogWarning($"Key: {error.Key}, Error: {subError.ErrorMessage}");
                    }
                }
            }

            var trasgressori = await Task.Run(() => _anagraficaService.GetAllTrasgressore());
            var violazioni = await Task.Run(() => _violazioneService.GetAllViolazioni());

            if (trasgressori == null || violazioni == null)
            {
                throw new InvalidOperationException("Trasgressori o Violazioni non possono essere nulli");
            }

        ViewBag.Trasgressori = trasgressori.Select(t => new { t.IDAnagrafica, NomeCompleto = $"{t.Nome} {t.Cognome}" });
        ViewBag.Violazioni = violazioni.Select(v => new { v.IDViolazione, v.Descrizione });
            return View(verbale);
        }

    }
}
