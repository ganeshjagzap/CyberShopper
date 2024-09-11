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

        /* [HttpGet]
         public IActionResult Login()
         {
             return View();
         }

         [HttpPost]
         public IActionResult Login(Customer model)
         {  
                if(ModelState.IsValid)
             {
                 var user = _custRepository.AuthenticUser(model.CustomerId, model.Password);
                 if (user != null)
                 {
                     HttpContext.Session.SetString("CustomerId", user.CustomerId.ToString());
                     _logger.LogInformation("User {CustomerId} logged in successfully.", user.CustomerId);
                     return RedirectToAction("Index");
                 }
                 ModelState.AddModelError("", "Invalid login attempt.");
                 _logger.LogWarning("Invalid login attempt for user {CustomerId}.", model.CustomerId);
             }
             return View(model);
         }

         [HttpGet]
         public IActionResult Register()
         {
             return View();
         }

         [HttpPost]
         public IActionResult Register([Bind("FullName", "EmailAddress", "Password", "DeliveryAddress")] Customer model)
         {
         public IActionResult Register(Customer model)
         {
             if (ModelState.IsValid)
             {

                 if (_custRepository.AuthenticUser(model.CustomerId, model.Password) == null)
                 {
                     _custRepository.AddUser(model);
                     _logger.LogInformation("User {CustomerId} registered successfully.", model.CustomerId);
                     return RedirectToAction("Login", "Home");
                 }
                 ModelState.AddModelError("", "User already exists.");
             }
             return View(model);
         }
         [HttpPost]
         public IActionResult Register(Customer model)
         {
             // Log the incoming model data
             _logger.LogInformation("Received Customer Data: {@Customer}", model);

             if (!ModelState.IsValid)
             {
                 // Log model state errors
                 foreach (var state in ModelState)
                 {
                     _logger.LogWarning("ModelState Error: Key={Key}, Errors={Errors}", state.Key, string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage)));
                 }
                 return View(model);
             }

             if (_custRepository.AuthenticUser(model.CustomerId, model.Password) == null)
             {
                 _custRepository.AddUser(model);
                 _logger.LogInformation("User {CustomerId} registered successfully.", model.CustomerId);
                 return RedirectToAction("Login", "Home");
             }

             ModelState.AddModelError("", "User already exists.");
             return View(model);
         }*/

        [HttpGet]
        public IActionResult Login(string email)
        {
            var customer = _custRepository.GetAll(email);
            return View(customer);  // Use the same model
        }

        [HttpPost]
        public IActionResult Login(Customer customer ,string password)
        {
            var isvalid = _custRepository.IsValide(customer,password);
            return RedirectToAction("Index");
        }


        /*[HttpPost]
        public IActionResult Login(Customer model)
        {
            // Log the received model data
            _logger.LogInformation("Login attempt with CustomerId: {CustomerId}", model.CustomerId);

            // Validate only required fields for login
            if (model.CustomerId <= 0 || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Customer ID and Password are required.");
            }

            if (!ModelState.IsValid)
            {
                // Log model state errors
                foreach (var state in ModelState)
                {
                    _logger.LogWarning("ModelState Error: Key={Key}, Errors={Errors}", state.Key, string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage)));
                }

                return View(model);
            }

            var user = _custRepository.AuthenticUser(model.CustomerId, model.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("CustomerId", user.CustomerId.ToString());
                _logger.LogInformation("User {CustomerId} logged in successfully.", user.CustomerId);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            _logger.LogWarning("Invalid login attempt for user {CustomerId}.", model.CustomerId);

            return View(model);
        }*/

        [HttpGet]
        public IActionResult Register()
        {
            return View(new Customer());  // Use the same model
        }

        [HttpPost]
        public IActionResult Register(Customer model)
        {
            // Log the incoming model data
            _logger.LogInformation("Received Registration Data: {@Customer}", model);

            // Validate registration fields
            if (string.IsNullOrEmpty(model.FullName) ||
                string.IsNullOrEmpty(model.EmailAddress) ||
                string.IsNullOrEmpty(model.DeliveryAddress))
            {
                ModelState.AddModelError("", "Full Name, Email Address, and Delivery Address are required.");
            }

            if (!ModelState.IsValid)
            {
                // Log model state errors
                foreach (var state in ModelState)
                {
                    _logger.LogWarning("ModelState Error: Key={Key}, Errors={Errors}", state.Key, string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage)));
                }
                return View(model);
            }

            if (_custRepository.AuthenticUser(model.CustomerId, model.Password) == null)
            {
                _custRepository.AddUser(model);
                _logger.LogInformation("User {CustomerId} registered successfully.", model.CustomerId);
                return RedirectToAction("Login", "Home");
            }

            ModelState.AddModelError("", "User already exists.");
            return View(model);
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Home");
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
