using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Customer GetAll(string email)
        {
            foreach (var customer in _context.Customers)
            {
                if (customer.EmailAddress == email)
                {
                    return customer;
                }
            }
            return null;
        }

        public bool IsValide(Customer customer,string password) {
            if (customer.Password == password)
            {
                return true;
            }
            return false;
            /*var isemail=_context.Customers.FirstOrDefault(e => e.EmailAddress == email);

            var ispassword = _context.Customers.FirstOrDefault(p => p.Password == password);
            if(isemail && ispassword)
            {
                return true;
            }
            return false;*/

        }

        public Customer AuthenticUser(int customerId, string password)
        {
            try
            {
                return _context.Customers.SingleOrDefault(u => u.CustomerId == customerId && u.Password == password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating user with ID {CustomerId}", customerId);
                return null;
            }
        }

        /*public void AddUser(Customer user)
        {
            try
            {
                _context.Customers.Add(user);
                _context.SaveChanges();
                _logger.LogInformation("User {CustomerId} added successfully.", user.CustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user with ID {CustomerId}", user.CustomerId);
            }
        }*/
        public void AddUser(Customer user)
        {
            try
            {
                _logger.LogInformation("Adding user with ID {CustomerId}", user.CustomerId);
                _context.Customers.Add(user);
                _context.SaveChanges();
                _logger.LogInformation("User {CustomerId} added successfully.", user.CustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user with ID {CustomerId}", user.CustomerId);
            }
        }
     

        public IEnumerable<Category> GetCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return Enumerable.Empty<Category>();
            }
        }

        //For Customer Controller
        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Find(id);
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();
        }

        public bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
