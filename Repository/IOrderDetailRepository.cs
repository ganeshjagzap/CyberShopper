using System.Collections.Generic;
using Ecommerce.Models;

namespace Ecommerce.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetAllOrderDetails();
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);
    }
}
