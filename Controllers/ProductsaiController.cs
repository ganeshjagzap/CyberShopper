using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class ProductsaiController : Controller
    {
        private readonly IProductsaiRepository _repository;


        public ProductsaiController(IProductsaiRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]

        public IActionResult Index(int categoryId)
        {
            var products = _repository.GetProductsByCategory(categoryId);
            return View(products);
        }

        public IActionResult Details()
        {
            var products = _repository.GetAllProducts();
            return View(products);
        }


        /*
                [HttpPost]
                public IActionResult AddToCart(int productId)
                {
                    _repository.AddProductToCart(productId);
                    // Redirect to a confirmation or cart page
                    return RedirectToAction("Index1", new { categoryId = _repository.GetProductById(productId)?.CategoryId });
                }
        */
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.AddProduct(product);
                return RedirectToAction("Index1");
            }
            return View(product);
        }



    }
}