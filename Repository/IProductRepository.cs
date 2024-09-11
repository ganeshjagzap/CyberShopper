using Ecommerce.Models;

namespace Ecommerce.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetPopularProducts();
    }
}
