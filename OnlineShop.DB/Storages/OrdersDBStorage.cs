using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Orders;

namespace OnlineShop.DB.Storages
{
    public class OrdersDBStorage : IOrdersStorage
    {
        private readonly DatabaseContext _databaseContext;
        public OrdersDBStorage(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Order> GetAll()
        {
            return _databaseContext.Orders
                .Include(user => user.DeliveryUser)
                .Include(items => items.Items)
                .ThenInclude(services => services.Services)
                .ToList();
        }

        public List<Order> TryGetAllByUserId(string userId)
        {
            return _databaseContext.Orders
                .Include(user => user.DeliveryUser)
                .Include(items => items.Items)
                .ThenInclude(services => services.Services)
                .Where(id => id.UserID == userId)
                .ToList();
        }

        public Order? TryGetById(Guid orderId)
        {
            return _databaseContext.Orders
                .Include(user => user.DeliveryUser)
                .Include(items => items.Items)
                .ThenInclude(services => services.Services)
                .FirstOrDefault(id => id.ID == orderId);
        }

        public void Add(Order order)
        {
            _databaseContext.Orders.Add(order);
            _databaseContext.SaveChanges();
        }
        public void UpdateStatus(Guid orderId, OrderStatus status)
        {
            var existingOrder = TryGetById(orderId);

            if (existingOrder != null)
            {
                existingOrder.Status = status;
                _databaseContext.SaveChanges();
            }
        }
    }
}
