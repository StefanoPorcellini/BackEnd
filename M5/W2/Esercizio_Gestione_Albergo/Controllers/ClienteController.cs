using Microsoft.AspNetCore.Mvc;
using Esercizio_Gestione_Albergo.ViewModels;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.Models;


namespace Esercizio_Gestione_Albergo.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteDAO _clienteDAO;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IClienteDAO clienteDAO, ILogger<ClienteController> logger)
        {
            _clienteDAO = clienteDAO;
            _logger = logger;
        }


        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            try
            {
                var clienti = await _clienteDAO.GetAll();
                var viewModels = clienti.Select(cliente => new ClienteViewModel
                {
                    CodiceFiscale = cliente.CodiceFiscale,
                    Cognome = cliente.Cognome,
                    Nome = cliente.Nome,
                    Città = cliente.Città,
                    Provincia = cliente.Provincia,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono,
                    Cellulare = cliente.Cellulare
                }).ToList();

                if (viewModels == null)
                {
                    return View("Error");
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero dei clienti.");
                return View("Error");
            }
        }

        // GET: Cliente/Details
        public async Task<IActionResult> Details(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = await _clienteDAO.GetByCodiceFiscale(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Edit
        public async Task<IActionResult> Edit(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = await _clienteDAO.GetByCodiceFiscale(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string codiceFiscale, ClienteViewModel viewModel)
        {
            if (codiceFiscale != viewModel.CodiceFiscale)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var cliente = new Cliente
                {
                    CodiceFiscale = viewModel.CodiceFiscale,
                    Cognome = viewModel.Cognome,
                    Nome = viewModel.Nome,
                    Città = viewModel.Città,
                    Provincia = viewModel.Provincia,
                    Email = viewModel.Email,
                    Telefono = viewModel.Telefono,
                    Cellulare = viewModel.Cellulare
                };

                await _clienteDAO.Update(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Cliente/Delete
        public async Task<IActionResult> Delete(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = await _clienteDAO.GetByCodiceFiscale(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string codiceFiscale)
        {
            try
            {
                await _clienteDAO.Delete(codiceFiscale);
                _logger.LogInformation($"Cliente con codice fiscale {codiceFiscale} eliminato con successo.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante l'eliminazione del cliente con codice fiscale {codiceFiscale}.");
                ModelState.AddModelError(string.Empty, "Errore durante l'eliminazione del cliente.");
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }

            return RedirectToAction(nameof(Index));
        }



        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente viewModel)
        {
            _logger.LogInformation("POST Create method called.");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is valid.");

                try
                {
                    var cliente = new Cliente
                    {
                        CodiceFiscale = viewModel.CodiceFiscale,
                        Cognome = viewModel.Cognome,
                        Nome = viewModel.Nome,
                        Città = viewModel.Città,
                        Provincia = viewModel.Provincia,
                        Email = viewModel.Email,
                        Telefono = viewModel.Telefono,
                        Cellulare = viewModel.Cellulare
                    };

                    await _clienteDAO.Add(cliente);
                    _logger.LogInformation("Cliente aggiunto con successo.");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiunta del cliente.");
                    ModelState.AddModelError(string.Empty, "Errore durante l'aggiunta del cliente.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState is not valid.");
            }

            return View(viewModel);
        }
    }
}
