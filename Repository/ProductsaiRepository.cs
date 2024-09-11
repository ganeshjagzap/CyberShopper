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

        [HttpGet]
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
            return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
            if (_products == null)
            {
                throw new InvalidOperationException("Product list is not initialized.");
            }
        }

        public Product GetProductById(int productId)
        {
            return _products.SingleOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(Product product)
        {
            if (_products.Any(p => p.ProductId == product.ProductId))
            {
                throw new InvalidOperationException("Product with this ID already exists.");
            }
            _products.Add(product);
        }



        public void AddProductToCart(int productId)
        {
            if (!_cart.Contains(productId))
            {
                _cart.Add(productId);
            }
        }

        //private readonly ApplicationDbContext _context;

        //public ProductRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public IEnumerable<Product> GetProductsByCategory(int categoryId)
        //{
        //    return _context.Products
        //                    .Where(p => p.CategoryId == categoryId)
        //                    .ToList();
        //}

        //public Product GetProductById(int productId)
        //{
        //    return _context.Products
        //                    .SingleOrDefault(p => p.ProductId == productId);
        //}
        //private readonly List<Product> _products;
        //private readonly List<int> _cart;
        //public IEnumerable<Product> GetProductsByCategory(int categoryId)
        //{
        //    return _products.Where(p => p.CategoryId == categoryId).ToList();
        //}

        //public Product GetProductById(int productId)
        //{
        //    return _products.SingleOrDefault(p => p.ProductId == productId);
        //}

        //public void AddProductToCart(int productId)
        //{
        //    if (!_cart.Contains(productId))
        //    {
        //        _cart.Add(productId);
        //    }
        //}

        //public class ProductRepository : IProductRepository
        //{
        //    private readonly ApplicationDbContext _context;
        //    public ProductRepository(ApplicationDbContext context)
        //    {
        //        _context = context;
        //    }
        //    public void AddProduct(Product product)
        //    {
        //        _context.Products.Add(product);
        //        _context.SaveChanges();
        //    }

        //    public void DeleteProduct(int productId)
        //    {
        //        //return _context.Products.ToList();
        //        var product = _context.Products.Find(productId);
        //        if (product != null)
        //        {
        //            _context.Products.Remove(product);
        //            _context.SaveChanges();
        //        }
        //    }

        //    public IEnumerable<Product> GetAllProducts()
        //    {
        //        //throw new NotImplementedException();
        //        return _context.Products.ToList();
        //    }

        //    public Product GetProductById(int productId)
        //    {
        //        return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        //    }


        //}
    }
}