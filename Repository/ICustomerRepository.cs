using  Ecommerce.Models;
namespace Ecommerce.Repository
{
    public interface ICustomerRepository
    {
        Customer AuthenticUser(int customerId, string password);
        Customer GetAll(string email);
        bool IsValide(Customer customer, string password);
        void AddUser(Customer customer);
        IEnumerable<Category> GetCategories();
        Customer GetCustomerById(int id);
        void UpdateCustomer(Customer customer);
        bool CustomerExists(int id);
    }
}
