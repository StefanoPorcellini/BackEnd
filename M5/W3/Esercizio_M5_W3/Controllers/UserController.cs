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

        [HttpGet]
        public IActionResult  CreateUser()
            {
                return View();
            }

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

                // Mostra il form di modifica per un cliente
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(int id, User updatedUserDetails)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Aggiorna le proprietà necessarie
                user.Name = updatedUserDetails.Name;
                user.Email = updatedUserDetails.Email;
                user.Password = updatedUserDetails.Password;
                user.Roles = updatedUserDetails.Roles;

                var updatedUser = await _userService.UpdateUserAsync(user);
                return RedirectToAction("UserDetails", new { id = updatedUser.Id });
            }
            return View(updatedUserDetails);
        }


        // Elimina un cliente
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
