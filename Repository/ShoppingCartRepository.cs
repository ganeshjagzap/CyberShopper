using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public ShoppingCart GetProductById(int productId)
    {
        return _context.ShoppingCarts.FirstOrDefault(p => p.ProductId == productId);
    }
        public Product GetProduct(int productId)
        {
            return _context.Products
                           .FirstOrDefault(p => p.ProductId == productId);
        }

        public IEnumerable<ShoppingCart> GetAllItemFromCart() {
            return _context.ShoppingCarts.Include(sc => sc.Product).ToList();
        }      

         public OrderDetail GetOrderDetail(int orderId) {
                        var productIdsInCart = _context.ShoppingCarts
                                             .Where(sc => sc.CartId != null) 
                                             .Select(sc => sc.ProductId)
                                             .Distinct()
                                             .ToList();

                          var orderDetail = _context.OrderDetails
                                        .Include(od => od.Product)
                                        .Where(od => od.OrderId == orderId && productIdsInCart.Contains(od.ProductId))
                                        .FirstOrDefault();

             return orderDetail;
         }

        public void AddToCart(Product product)
        {
            
            var existingCartItem = _context.ShoppingCarts
                                           .FirstOrDefault(sc => sc.ProductId == product.ProductId ); // Use actual CartId logic

            if (existingCartItem != null)
            {
                
                existingCartItem.Quantity += 1;
                _context.Update(existingCartItem);
            }
            else
            {               
                var newCartItem = new ShoppingCart
                {
                    CartId = "DefaultCartId",
                    ProductId = product.ProductId,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                _context.ShoppingCarts.Add(newCartItem);
            }
            _context.SaveChanges();
        }

        public void RemoveFromCart(ShoppingCart item)
        {
            var cartItem = _context.ShoppingCarts
                                   .FirstOrDefault(sc => sc.ProductId == item.ProductId); 

            if (cartItem != null)
            {
                _context.ShoppingCarts.Remove(cartItem);
                _context.SaveChanges();
            }
        }
        public void UpdateCart(ShoppingCart product, int quantity)
        {
            var cartItem = _context.ShoppingCarts
                                   .FirstOrDefault(sc => sc.ProductId == product.ProductId); 

            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                    _context.Update(cartItem);
                }
                else
                {
                    _context.ShoppingCarts.Remove(cartItem);
                }
                _context.SaveChanges();
            }
        }
        public void PlaceOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _context.ShoppingCarts.ToList();
            _context.ShoppingCarts.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public void SaveOrderToHistory(Order order)
        {
            var orderHistory = new OrderHistory
            {
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                OrderDetails = order.OrderDetails.ToList()
            };

            _context.OrderHistories.Add(orderHistory);
            _context.SaveChanges();
        }

        public IEnumerable<OrderHistory> GetOrderHistory(int customerId)
        {
            return _context.OrderHistories
                           .Include(oh => oh.OrderDetails)
                           .ThenInclude(od => od.Product)
                           .Where(oh => oh.CustomerId == customerId)
                           .ToList();
        }

        public OrderHistory GetOrderHistoryById(int orderHistoryId)
        {
            return _context.OrderHistories
                           .Include(oh => oh.OrderDetails)
                           .ThenInclude(od => od.Product)
                           .FirstOrDefault(oh => oh.OrderHistoryId == orderHistoryId);
        }



    }
}
