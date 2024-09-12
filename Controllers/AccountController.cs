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

        public IActionResult Login()
        {
            return View();
        }

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

           
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, customer.EmailAddress),
        new Claim(ClaimTypes.Name, customer.FullName),
        new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()) // Ensure this claim is set
    };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

            
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties).GetAwaiter().GetResult();

            HttpContext.Session.SetString("UserEmail", customer.EmailAddress);
            HttpContext.Session.SetString("UserFullName", customer.FullName);

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


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
                Password = model.Password,
                DeliveryAddress = model.DeliveryAddress
            };

            _accountRepository.AddCustomer(customer);
            if (_accountRepository.SaveChanges())
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, customer.EmailAddress),
                    new Claim(ClaimTypes.Name, customer.FullName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = false };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties).GetAwaiter().GetResult();

                HttpContext.Session.SetString("UserEmail", customer.EmailAddress);
                HttpContext.Session.SetString("UserFullName", customer.FullName);
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Failed to register. Please try again.");
            return View(model);
        }

    }
}
