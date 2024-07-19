using Esercizio_M5_W1.Models;
using Esercizio_M5_W1.Services.V1;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_M5_W1.Controllers
{
    public class SpedizioneController : Controller
    {
        private readonly ISpedizioneService _spedizioneService;

        public SpedizioneController(ISpedizioneService spedizioneService)
        {
            _spedizioneService = spedizioneService;
        }

        public IActionResult Create(Spedizione spedizione)
        {
            try
            {
                _spedizioneService.CreaSpedizione(spedizione);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating shipment: {ex.Message}");
                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
