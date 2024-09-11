using Ecommerce.Models;

namespace Ecommerce.Repository
{
    public interface IProductsaiRepository
    {
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        Product GetProductById(int productId);
        void AddProduct(Product product);
        //void UpdateProduct(Product product);
       // void AddProductToCart(int productId);
        IEnumerable<Product> GetAllProducts();

    }
}