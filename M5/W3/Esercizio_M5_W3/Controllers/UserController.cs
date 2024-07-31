using Esercizio_Pizzeria_In_Forno.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Esercizio_Pizzeria_In_Forno.Models;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // Crea un nuovo cliente
    [HttpGet]
    public IActionResult CreateUser()
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
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok();
    }

    // Login

    [HttpGet]
        public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            var user = await _userService.LoginAsync(email, password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Errore di Login.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Errore durante il processo di Login.");
            return View();
        }        
    }

    //logout

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
