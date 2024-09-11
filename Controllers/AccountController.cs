using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Http;
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
                new Claim(ClaimTypes.Name, customer.FullName)
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

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingCustomer = _accountRepository.GetCustomerByEmail(model.EmailAddress);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("", "Email address is already registered.");
                return View(model);
            }

            var customer = new Customer
            {
                FullName = model.FullName,
                EmailAddress = model.EmailAddress,
                Password = model.Password, // Replace with hashed password
                DeliveryAddress = model.DeliveryAddress
            };

            _accountRepository.AddCustomer(customer);
            if (_accountRepository.SaveChanges())
            {
                // Optionally set authentication cookie here
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, customer.EmailAddress),
                    new Claim(ClaimTypes.Name, customer.FullName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = false };

                // Sign in the user synchronously
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties).GetAwaiter().GetResult();

                HttpContext.Session.SetString("UserEmail", customer.EmailAddress);
                HttpContext.Session.SetString("UserFullName", customer.FullName);
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Failed to register. Please try again.");
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Sign out the user synchronously
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
