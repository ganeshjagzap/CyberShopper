using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

      
        public IActionResult Details()
        {
            int userId = 1;
            var customer = _customerRepository.GetCustomerById(userId);

            if (customer == null)
            {
                return NotFound("No data");
            }

            return View(customer);
        }

        public IActionResult Edit()
        {
            int userId = 1;
            var customer = _customerRepository.GetCustomerById(userId);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                return View(customer);
            }
            try
            {
                _customerRepository.UpdateCustomer(customer);
            }
            catch (DbUpdateConcurrencyException)
            { 
                if (!_customerRepository.CustomerExists(customer.CustomerId))
                {
                    return NotFound();
                }
                else
                {
                   throw;
                }
            }
            return RedirectToAction(nameof(Details));
        }
    }
}
