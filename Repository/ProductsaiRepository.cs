using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Repository
{
    public class ProductsaiRepository : IProductsaiRepository
    {
        private readonly List<Product> _products;
        private readonly List<int> _cart; // To store product IDs in the cart
        private readonly ApplicationDbContext _context;

        public ProductsaiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public ProductsaiRepository()
        {
            _products = new List<Product>();
            _cart = new List<int>(); // Initialize an empty cart
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products
                                .Where(p => p.CategoryId == categoryId)
                                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.SingleOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(Product product)
        {
            if (_products.Any(p => p.ProductId == product.ProductId))
            {
                throw new InvalidOperationException("Product with this ID already exists.");
            }
            _products.Add(product);
        }



        /* public void AddProductToCart(int productId)
         {
             if (!_cart.Contains(productId))
             {
                 _cart.Add(productId);
             }
         }*/


    }
}