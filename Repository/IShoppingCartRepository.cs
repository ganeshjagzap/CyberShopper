using Ecommerce.Models;
namespace Ecommerce.Repository
{
    public interface IShoppingCartRepository
    {
        IEnumerable<ShoppingCart> GetAllItemFromCart();

        OrderDetail GetOrderDetail(int orderId);

        void AddToCart(Product product);
        void RemoveFromCart(ShoppingCart item);
        void UpdateCart(ShoppingCart product, int quantity);
        IEnumerable<Product> GetAllProducts();
        ShoppingCart GetProductById(int productId);
        Product GetProduct(int productId);

        void PlaceOrder(Order order);

        void ClearCart();

        void SaveOrderToHistory(Order order);
        IEnumerable<OrderHistory> GetOrderHistory(int customerId);
        OrderHistory GetOrderHistoryById(int orderHistoryId);




    }
}
