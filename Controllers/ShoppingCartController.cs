using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Repository;
using Ecommerce.Models;
using System.Linq;
using System.Security.Claims;
using System;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IActionResult Index()
        {
            var items = _shoppingCartRepository.GetAllItemFromCart();
            return View(items);
        }

        public IActionResult AllProducts()
        {
            var products = _shoppingCartRepository.GetAllProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _shoppingCartRepository.GetProduct(productId);
            if (product != null)
            {
                _shoppingCartRepository.AddToCart(product);
            }
            return RedirectToAction("Index", "Home");
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

        [HttpPost]
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
            // Retrieve the current user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int customerId))
            {
                var items = _shoppingCartRepository.GetAllItemFromCart();

                if (items.Any())
                {
                    var order = new Order
                    {
                        CustomerId = customerId, // Use the actual customer ID
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
                    _shoppingCartRepository.ClearCart();

                    return RedirectToAction("OrderConfirmation");
                }
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int customerId))
            {
                var orderHistory = _shoppingCartRepository.GetOrderHistory(customerId);
                return View(orderHistory);
            }

            return RedirectToAction("Index");
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
