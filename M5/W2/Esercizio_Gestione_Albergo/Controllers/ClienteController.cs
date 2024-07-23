using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.ViewModels;
using Esercizio_Gestione_Albergo.Services.DAO;

namespace Esercizio_Gestione_Albergo.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteDAO _clienteDao;

        public ClienteController(IClienteDAO clienteDao)
        {
            _clienteDao = clienteDao;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            try
            {
                // Recupera tutti i clienti dal DAO
                var clienti = await _clienteDao.GetAllAsync();

                // Converti i clienti in view model
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
                    // Gestisci il caso in cui la lista è null
                    return View("Error"); // O una vista più appropriata
                }

                // Passa i view model alla vista
                return View(viewModels);
            }
            catch (Exception ex)
            {
                // Gestione dell'errore, puoi loggare l'errore e mostrare una vista di errore
                // Log.Error(ex, "Errore durante il recupero dei clienti");
                return View("Error"); // Sostituisci con una vista di errore appropriata
            }
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = await _clienteDao.GetByCodiceFiscaleAsync(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel viewModel)
        {
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

                await _clienteDao.AddAsync(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = await _clienteDao.GetByCodiceFiscaleAsync(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Edit/5
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

                await _clienteDao.UpdateAsync(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = await _clienteDao.GetByCodiceFiscaleAsync(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string codiceFiscale)
        {
            await _clienteDao.DeleteAsync(codiceFiscale);
            return RedirectToAction(nameof(Index));
        }
    }

}
