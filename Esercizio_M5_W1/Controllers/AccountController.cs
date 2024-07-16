using Esercizio_M5_W1.Models;
using Esercizio_M5_W1.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml;

namespace Esercizio_M5_W1.Controllers
{
    public class AccountController : Controller
    {
        public readonly IAuthService authenticationServices;

        public AccountController(IAuthService authenticationServices)
        {
            this.authenticationServices = authenticationServices;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            try
            {var u = authenticationServices.Login(user.Username, user.Password);
                if (u == null) return RedirectToAction("Index", "Home");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, u.Username),
                };
                u.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
            catch (Exception ex) { }
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
