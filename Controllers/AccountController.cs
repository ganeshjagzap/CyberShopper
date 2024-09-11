using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;

using Ecommerce.Repository;

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
            if (customer == null || customer.Password != model.Password) // Replace with hashed password comparison
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // Set authentication cookie here
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
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Failed to register. Please try again.");
            return View(model);
        }
    }
}