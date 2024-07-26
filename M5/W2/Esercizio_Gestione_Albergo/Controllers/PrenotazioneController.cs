using Microsoft.AspNetCore.Mvc;
using Esercizio_Gestione_Albergo.ViewModels;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;

namespace Esercizio_Gestione_Albergo.Controllers
{
    public class PrenotazioniController : Controller
    {
        private readonly IPrenotazioneDAO _prenotazioneDAO;
        private readonly ILogger<PrenotazioniController> _logger;

        public PrenotazioniController(IPrenotazioneDAO prenotazioneDAO, ILogger<PrenotazioniController> logger)
        {
            _prenotazioneDAO = prenotazioneDAO;
            _logger = logger;
        }

        // GET: Prenotazioni
        public async Task<IActionResult> Index()
        {
            try
            {
                var prenotazioni = await _prenotazioneDAO.GetAllAsync();
                return View(prenotazioni);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero delle prenotazioni.");
                return StatusCode(500, "Errore interno del server.");
            }
        }

        // GET: Prenotazioni/Details
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
                if (prenotazione == null)
                {
                    return NotFound();
                }
                return View(prenotazione);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero dei dettagli della prenotazione.");
                return StatusCode(500, "Errore interno del server.");
            }
        }

        // GET: Prenotazioni/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prenotazioni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
            "ClienteCodiceFiscale, CameraNumero, DataPrenotazione, NumeroProgressivo, Anno, Dal, Al, CaparraConfirmatoria, " +
            "Tariffa, DettagliSoggiornoId, SaldoFinale")] Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(
                        "Creazione di una nuova prenotazione con ClienteCodiceFiscale: {ClienteCodiceFiscale}, CameraNumero: {CameraNumero}.",
                        prenotazione.ClienteCodiceFiscale, prenotazione.CameraNumero);

                    // Verifica la disponibilità della camera
                    bool isAvailable = await _prenotazioneDAO.IsCameraAvailableAsync(
                        prenotazione.CameraNumero, prenotazione.Dal, prenotazione.Al);
                    if (!isAvailable)
                    {
                        ModelState.AddModelError("", "La camera non è disponibile per le date selezionate.");
                        _logger.LogWarning("La camera {CameraNumero} non è disponibile per le date dal {Dal} al {Al}.",
                            prenotazione.CameraNumero, prenotazione.Dal, prenotazione.Al);

                        return View(prenotazione);
                    }

                    // Aggiungi la prenotazione
                    await _prenotazioneDAO.AddAsync(prenotazione);
                    _logger.LogInformation("Prenotazione creata con successo.");

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
            try
            {
                var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
                if (prenotazione == null)
                {
                    return NotFound();
                }
                return View(prenotazione);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della prenotazione per l'edit.");
                return StatusCode(500, "Errore interno del server.");
            }
        }

        // POST: Prenotazioni/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClienteCodiceFiscale,CameraNumero,DataPrenotazione,NumeroProgressivo,Anno,Dal,Al,CaparraConfirmatoria,Tariffa,DettagliSoggiornoId")] PrenotazioneViewModel prenotazione)
        {
            if (id != prenotazione.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _prenotazioneDAO.UpdateAsync(prenotazione);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Errore durante l'aggiornamento della prenotazione: {ex.Message}");
                    _logger.LogError(ex, "Errore durante l'aggiornamento della prenotazione.");
                }
            }
            return View(prenotazione);
        }

        // GET: Prenotazioni/Delete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
                if (prenotazione == null)
                {
                    return NotFound();
                }
                return View(prenotazione);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della prenotazione per la cancellazione.");
                return StatusCode(500, "Errore interno del server.");
            }
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
                _logger.LogError(ex, "Errore durante l'eliminazione della prenotazione.");

                var prenotazione = await _prenotazioneDAO.GetByIdAsync(id);
                return View("Delete", prenotazione);
            }
        }
    }
}
