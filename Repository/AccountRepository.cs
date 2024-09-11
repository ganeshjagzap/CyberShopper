using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models.ViewModels;

namespace Ecommerce.Repository
{
    

    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _context.Customers.SingleOrDefault(c => c.EmailAddress == email);
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Find(id);
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }



}
