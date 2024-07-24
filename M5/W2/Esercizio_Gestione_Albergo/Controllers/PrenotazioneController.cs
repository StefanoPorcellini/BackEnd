using Microsoft.AspNetCore.Mvc;
using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.Controllers
{
    public class PrenotazioniController : Controller
    {
        private readonly PrenotazioneDAO _prenotazioneDAO;

        public PrenotazioniController(IConfiguration configuration)
        {
            _prenotazioneDAO = new PrenotazioneDAO(configuration);
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
        public async Task<IActionResult> Create([Bind("ID,ClienteCodiceFiscale,CameraNumero,DataPrenotazione,NumeroProgressivo,Anno,Dal,Al," +
                                                      "CaparraConfirmatoria,Tariffa,DettagliSoggiornoId")] PrenotazioneViewModel prenotazione)
        {
            if (ModelState.IsValid)
            {
                await _prenotazioneDAO.AddAsync(prenotazione);
                return RedirectToAction(nameof(Index));
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
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("", "An error occurred while updating the record. Please try again later.");
                    return View(prenotazione);
                }
                return RedirectToAction(nameof(Index));
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
            await _prenotazioneDAO.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
