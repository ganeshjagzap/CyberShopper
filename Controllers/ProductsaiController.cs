using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize]
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