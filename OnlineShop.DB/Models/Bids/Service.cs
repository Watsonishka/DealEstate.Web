using OnlineShop.DB.Models.Orders;

namespace OnlineShop.DB.Models.Bids
{
    public class Service
    {
        public Guid ID { get; set; }
        public bool IsRemoved { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public BidItem? BidItem { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
