using Microsoft.AspNetCore.Mvc;
using Ecommerce.Repository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orderDetails = _orderDetailRepository.GetAllOrderDetails();
            return View(orderDetails);
        }

        // GET: OrderDetail/Details/5
        public IActionResult Details(int orderId)
        {
            var orderDetails = _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }
            return View(orderDetails);
        }
    }
}