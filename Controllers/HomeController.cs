using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Repository;
using Ecommerce.Models;
using System.Linq;
using System.Security.Claims;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    [Authorize]
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
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int customerId))
            {
                var items = _productRepository.GetPopularProducts();
                ViewBag.PopularProducts = items;
                return View();
            }
            return RedirectToAction("Login","Account");            
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