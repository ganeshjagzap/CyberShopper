using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerRepository _custRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, ICustomerRepository custRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _custRepository = custRepository;
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(_productRepository));
        }
        public IActionResult Index()
        {
            var items = _productRepository.GetPopularProducts();
            ViewBag.PopularProducts = items;

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
