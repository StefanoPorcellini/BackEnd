using Esercizio_M5_W1.Models;
using Esercizio_M5_W1.Services.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Esercizio_M5_W1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
        }

        // GetAll
        public IActionResult Index()
        {
            var clienti = _clienteService.GetAll();
            return View(clienti);
        }

        // Create
        public IActionResult Create()
        {
            return View();
        }

        // Create http post
        [HttpPost]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _clienteService.Create(cliente);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Errore durante la creazione del cliente: {ex.Message}");
                }
            }
            return View(cliente);
        }

        // Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cliente = _clienteService.GetById(id.Value);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante il recupero del cliente: {ex.Message}");
                return View();
            }
        }


        // Edit http post
        [HttpPost]
        public IActionResult Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _clienteService.Update(cliente);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Errore durante l'aggiornamento del cliente: {ex.Message}");
                }
            }
            return View(cliente);
        }

        ///Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cliente = _clienteService.GetById(id.Value);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante il recupero del cliente: {ex.Message}");
                return View();
            }
        }

        // Delete http post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _clienteService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante l'eliminazione del cliente: {ex.Message}");
                return View();
            }
        }
    }
}
