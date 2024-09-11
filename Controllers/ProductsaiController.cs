using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.CodeAnalysis;

namespace Ecommerce.Controllers
{
    public class ProductsaiController : Controller
    {
        private readonly IProductsaiRepository _repository;


        public ProductsaiController(IProductsaiRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index1(int categoryId)
        {
            var products = _repository.GetProductsByCategory(categoryId);
            return View(products);
        }
        public IActionResult Details()
        {
            var products = _repository.GetAllProducts();
            return View(products);
        }

        // public IActionResult Details()
        // {
        //try
        //{
        //    var product = _repository.GetProductById(productId);
        //    if (product == null)
        //    {
        //        // If the product is not found, set a friendly error message
        //        ViewBag.ErrorMessage = "Product not found.";
        //        return View("Error"); // You need to create an Error.cshtml view for this
        //    }
        //    return View(product);
        //}
        //catch (Exception ex)
        //{
        //    // Log the exception details (consider using a logging framework)
        //    Console.WriteLine($"An error occurred: {ex.Message}");

        //    // Set a friendly error message
        //    ViewBag.ErrorMessage = "An unexpected error occurred while retrieving the product details.";
        //    return View("Error"); // You need to create an Error.cshtml view for this
        //}
        // }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            _repository.AddProductToCart(productId);
            // Redirect to a confirmation or cart page
            return RedirectToAction("Index1", new { categoryId = _repository.GetProductById(productId)?.CategoryId });
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

        //[HttpGet]
        //public IActionResult UpdateProduct(int productId)
        //{
        //    var product = _repository.GetProductById(productId);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult UpdateProduct(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _repository.UpdateProduct(product);
        //        return RedirectToAction("Details", new { productId = product.ProductId });
        //    }
        //    return View(product);
        //}
        //private readonly IProductRepository _productRepository;
        //private readonly ICartRepository _cartRepository;

        //public ProductController(IProductRepository productRepository, ICartRepository cartRepository)
        //{
        //    _productRepository = productRepository;
        //    _cartRepository = cartRepository;
        //}

        //// GET: Product
        //public IActionResult Index(int categoryId)
        //{
        //    var products= _productRepository.GetProductsByCategory(categoryId);
        //    return View(products);
        //}

        //// GET: Product/Details/5
        //public IActionResult Details(int id)
        //{

        //    var product = _productRepository.GetProductById(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}
        //public ActionResult AddToCart(int productId)
        //{
        //    _cartRepository.AddToCart(productId);
        //    return RedirectToAction("Index", "Cart");
        //}
        //private readonly IProductRepository _repository;

        //public ProductController(IProductRepository repository)
        //{
        //    _repository = repository;
        //}


        //public IActionResult Index(int categoryId)
        //{
        //    var products = _repository.GetProductsByCategory(categoryId);
        //    return View(products);
        //}

        //public IActionResult Details(int productId)
        //{
        //    var product = _repository.GetProductById(productId);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult AddToCart(int productId)
        //{
        //    _repository.AddProductToCart(productId);
        //    // Assuming you want to redirect to the cart or a confirmation page
        //    return RedirectToAction("Index", new { categoryId = _repository.GetProductById(productId)?.CategoryId });
        //}


    }
}