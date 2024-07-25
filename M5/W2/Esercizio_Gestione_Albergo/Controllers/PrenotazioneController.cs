using Microsoft.AspNetCore.Mvc;
using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.ViewModels;
using Esercizio_Gestione_Albergo.Models;

namespace Esercizio_Gestione_Albergo.Controllers
{
    public class PrenotazioniController : Controller
    {
        private readonly PrenotazioneDAO _prenotazioneDAO;
        private readonly ILogger<PrenotazioniController> _logger;


        public PrenotazioniController(IConfiguration configuration, ILogger<PrenotazioniController> logger)
        {
            _prenotazioneDAO = new PrenotazioneDAO(configuration);
            _logger = logger;
        }

        // GET: Prenotazioni
        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneDAO.GetAllAsync();
            return View(prenotazioni);
        }

        // GET: Prenotazioni/Details
        public async Task<IActionResult> Details(int id)
        {
            var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View(prenotazione);
        }

        // GET: Prenotazioni/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prenotazioni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> 
            Create([Bind("ClienteCodiceFiscale,CameraNumero,Dal,Al,CaparraConfirmatoria,Tariffa,DettagliSoggiornoId")] Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creazione di una nuova prenotazione con ClienteCodiceFiscale: " +
                    "{ClienteCodiceFiscale}, CameraNumero: {CameraNumero}.", prenotazione.ClienteCodiceFiscale, prenotazione.CameraNumero);

                // Verifica la disponibilità della camera
                bool isAvailable = await _prenotazioneDAO.IsCameraAvailable(prenotazione.CameraNumero, prenotazione.Dal, prenotazione.Al);
                if (!isAvailable)
                {
                    ModelState.AddModelError("", "La camera non è disponibile per le date selezionate.");
                    _logger.LogWarning("La camera {CameraNumero} non è disponibile per le date dal {Dal} al {Al}.",
                        prenotazione.CameraNumero, prenotazione.Dal, prenotazione.Al);

                    return View(prenotazione);
                }

                try
                {
                    // Aggiungi la prenotazione
                    await _prenotazioneDAO.AddAsync(prenotazione);
                    _logger.LogInformation("Prenotazione creata con successo, ID: {PrenotazioneId}.", prenotazione.ID);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Errore durante la creazione della prenotazione: {ex.Message}");
                    _logger.LogError(ex, "Errore durante la creazione della prenotazione.");

                }
            }
            return View(prenotazione);
        }

        // GET: Prenotazioni/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View(prenotazione);
        }

        // POST: Prenotazioni/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClienteCodiceFiscale,CameraNumero,DataPrenotazione,NumeroProgressivo,Anno,Dal,Al," +
                                                            "CaparraConfirmatoria,Tariffa,DettagliSoggiornoId")] PrenotazioneViewModel prenotazione)
        {
            if (id != prenotazione.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Aggiorna la prenotazione nel database
                    await _prenotazioneDAO.UpdateAsync(prenotazione);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Errore durante l'aggiornamento della prenotazione: {ex.Message}");
                }
            }
            return View(prenotazione);
        }

        // GET: Prenotazioni/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View(prenotazione);
        }

        // POST: Prenotazioni/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _prenotazioneDAO.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante l'eliminazione della prenotazione: {ex.Message}");
                var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
                return View("Delete", prenotazione);
            }
        }
    }
}
