using Esercizio_M5_W1.Models;
using Esercizio_M5_W1.Services.Auth;
using Esercizio_M5_W1.Services.V1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Esercizio_M5_W1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authenticationServices;
        private readonly ISpedizioneService _spedizioneService;

        public AccountController(IAuthService authenticationServices, ISpedizioneService spedizioneService)
        {
            _authenticationServices = authenticationServices;
            _spedizioneService = spedizioneService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                var u = _authenticationServices.Login(user.Username, user.Password);
                if (u == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, u.Username),
                    new Claim(ClaimTypes.NameIdentifier, u.Id.ToString())


                };
                u.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                var spedizioni = _spedizioneService.GetShipById(u.Id);

                TempData["Spedizioni"] = Newtonsoft.Json.JsonConvert.SerializeObject(spedizioni);

                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }
           
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
    }
}
