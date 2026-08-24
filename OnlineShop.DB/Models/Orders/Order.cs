using OnlineShop.DB.Models.Users;

namespace OnlineShop.DB.Models.Orders
{
    public class Order
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }
        public DeliveryUser DeliveryUser { get; set; }
        public User User { get; set; }
    }
}

