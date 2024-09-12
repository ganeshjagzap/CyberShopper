using System.Collections.Generic;
using System.Linq;
using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<OrderDetail> GetAllOrderDetails()
        {
            
            return _context.OrderDetails.ToList();
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return [.. _context.OrderDetails.Where(od => od.OrderId == orderId)];
        }
    }
}
