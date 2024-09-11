using Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

     
        public IActionResult Index()
        {
            var items=_shoppingCartRepository.GetAllItemFromCart();
            return View(items);
        }


        public IActionResult AllProducts()
        {
            var products = _shoppingCartRepository.GetAllProducts();
            return View(products);
        }


        public IActionResult AddToCart(int productId)
        {
            var product = _shoppingCartRepository.GetProduct(productId);
            if (product != null)
            {
                _shoppingCartRepository.AddToCart(product);
            }
            return RedirectToAction("Index","Home");
        }

      
        [HttpGet]
        public IActionResult UpdateCart(int productId)
        {
            var item = _shoppingCartRepository.GetProductById(productId);
            return View(item);
        }
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
           var product = _shoppingCartRepository.GetProductById(productId);
            if (product != null)
            {

                _shoppingCartRepository.UpdateCart(product, quantity);

            }
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult RemoveFromCart(int productId)
        {

            var item = _shoppingCartRepository.GetProductById(productId);
            return View(item);
        }

        public IActionResult RemoveFromCart(ShoppingCart item)
        {
            
            _shoppingCartRepository.RemoveFromCart(item);
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Checkout()
        {
            var items = _shoppingCartRepository.GetAllItemFromCart();
            var totalAmount = items.Sum(item => item.Quantity * item.Product.UnitCost);

            ViewBag.Items = items;
            ViewBag.TotalAmount = totalAmount;

            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var items = _shoppingCartRepository.GetAllItemFromCart();

            if (items.Any())
            {
                var order = new Order
                {
                    CustomerId = 1, // Set this to the actual customer ID
                    OrderDate = DateTime.Now,
                    OrderDetails = items.Select(item => new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitCost = item.Product.UnitCost
                    }).ToList()
                };

                _shoppingCartRepository.PlaceOrder(order);
                _shoppingCartRepository.SaveOrderToHistory(order); // Save to history

                // Clear the shopping cart after placing the order
                _shoppingCartRepository.ClearCart();

                return RedirectToAction("OrderConfirmation");
            }

            return RedirectToAction("Index");
        }


        public IActionResult OrderConfirmation()
        {
            ViewData["Message"] = "Your order has been placed successfully!";
            return View();
        }
        [HttpGet]
        public IActionResult OrderHistory()
        {
            var customerId = 1; // Set this to the actual customer ID
            var orderHistory = _shoppingCartRepository.GetOrderHistory(customerId);
            return View(orderHistory);
        }


        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            var orderHistory = _shoppingCartRepository.GetOrderHistoryById(id);
            if (orderHistory == null)
            {
                return NotFound();
            }
            return View(orderHistory);
        }




    }
}
