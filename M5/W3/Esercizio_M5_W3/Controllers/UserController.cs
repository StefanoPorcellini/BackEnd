using Esercizio_Pizzeria_In_Forno.Models;
using Microsoft.AspNetCore.Mvc;
using Esercizio_Pizzeria_In_Forno.Service;

namespace Esercizio_Pizzeria_In_Forno.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Crea un nuovo cliente
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return RedirectToAction("UserDetails", new { id = createdUser.Id });
            }
            return View(user);
        }

        // Dettagli di un cliente specifico
        [HttpGet]
        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Ottieni tutti i clienti
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // Aggiorna un cliente
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (ModelState.IsValid)
            {
                var updatedUser = await _userService.UpdateUserAsync(user);
                return RedirectToAction("UserDetails", new { id = updatedUser.Id });
            }
            return View(user);
        }

        // Elimina un cliente
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("GetAllUsers");
        }
    }
}
