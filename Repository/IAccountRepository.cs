using System.Linq;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;

namespace Ecommerce.Repository
{
    public interface IAccountRepository
    {
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        bool SaveChanges();
    }


}
