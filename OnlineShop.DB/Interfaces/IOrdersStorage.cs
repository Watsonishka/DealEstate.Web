using OnlineShop.DB.Models.Orders;

namespace OnlineShop.DB.Interfaces
{
    public interface IOrdersStorage
    {      
        List<Order> GetAll();
        List<Order> TryGetAllByUserId(string userId);
        Order? TryGetById(Guid orderId);
        void Add(Order order);
        void UpdateStatus(Guid orderId, OrderStatus status);

    }
}
