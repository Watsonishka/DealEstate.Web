using OnlineShop.DB.Models.Bids;

namespace OnlineShop.DB.Models.Orders
{
    public class OrderItem
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCity { get; set; }
        public double ProductArea { get; set; }
        public decimal CurrentPrice { get; set; }
        public Order Order { get; set; }
        public List<Service> Services { get; set; }
    }
}
