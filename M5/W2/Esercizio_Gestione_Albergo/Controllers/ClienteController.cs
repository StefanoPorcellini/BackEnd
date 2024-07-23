using Microsoft.AspNetCore.Mvc;
using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.ViewModels;
using Esercizio_Gestione_Albergo.Services.DAO;

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
        public IActionResult Index()
        {
            try
            {
                // Recupera tutti i clienti dal DAO
                var clienti = _clienteDAO.GetAll();

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
        public IActionResult Details(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = _clienteDAO.GetByCodiceFiscale(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }
                
        // GET: Cliente/Edit/5
        public IActionResult Edit(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = _clienteDAO.GetByCodiceFiscale(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string codiceFiscale, ClienteViewModel viewModel)
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

                _clienteDAO.Update(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Cliente/Delete/5
        public IActionResult Delete(string codiceFiscale)
        {
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                return NotFound();
            }

            var cliente = _clienteDAO.GetByCodiceFiscale(codiceFiscale);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string codiceFiscale)
        {
            _clienteDAO.Delete(codiceFiscale);
            return RedirectToAction(nameof(Index));
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            _logger.LogInformation("GET Create method called.");

            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente viewModel)
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

                            _clienteDAO.Add(cliente);
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
