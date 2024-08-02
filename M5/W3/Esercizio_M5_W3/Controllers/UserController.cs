using Esercizio_Pizzeria_In_Forno.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Esercizio_Pizzeria_In_Forno.Context;
using Esercizio_Pizzeria_In_Forno.Models.ViewModels;

public class UserController : Controller
{
    private readonly DataContext _context;
    private readonly IUserService _userService;

    public UserController(DataContext context, IUserService userService)
    {
        _userService = userService;
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult AreaRiservata()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> CreateUser()
    {
        var roles = await _context.Roles.ToListAsync();
        var viewModel = new UserViewModel
        {
            Roles = roles
        };
        return View(viewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(UserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = viewModel.User;
            user.UserRoles = new List<UserRole>();

            if (User.IsInRole("Admin"))
            {
                user.UserRoles.Add(new UserRole { RoleId = viewModel.SelectedRoleId });
            }
            else
            {
                user.UserRoles.Add(new UserRole { RoleId = 2 }); 
            }

            await _userService.CreateUserAsync(user);
            return RedirectToAction("Index", "Home");
        }

        viewModel.Roles = await _context.Roles.ToListAsync();
        return View(viewModel);
    }


    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> UpdateUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var roles = await _context.Roles.ToListAsync();
        var viewModel = new UserViewModel
        {
            User = user,
            Roles = roles
        };

        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(UserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetUserByIdAsync(viewModel.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = viewModel.User.Name;
            user.Email = viewModel.User.Email;
            user.Password = viewModel.User.Password;

            // Aggiorna i ruoli
            user.UserRoles = viewModel.User.UserRoles;

            var updatedUser = await _userService.UpdateUserAsync(user);
            return RedirectToAction(nameof(UserDetails), new { id = updatedUser.Id });
        }

        viewModel.Roles = await _context.Roles.ToListAsync();
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(GetAllUsers));
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
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

            claims.AddRange(user.UserRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole.Role.Name)));

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

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
