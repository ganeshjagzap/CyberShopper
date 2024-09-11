using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = _accountRepository.GetCustomerByEmail(model.EmailAddress);
            if (customer == null || customer.Password != model.Password)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Set authentication cookie
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, customer.EmailAddress),
        new Claim(ClaimTypes.Name, customer.FullName),
        new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()) // Ensure this claim is set
    };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

            // Sign in the user synchronously
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties).GetAwaiter().GetResult();

            HttpContext.Session.SetString("UserEmail", customer.EmailAddress);
            HttpContext.Session.SetString("UserFullName", customer.FullName);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Sign out the user synchronously
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
