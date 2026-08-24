using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Models.Bids
{
    public class BidItem
    {
        public Guid ID { get; set; }
        public Product Product { get; set; }
        public List<Service>? Services { get; set; } 
        public Bid Bid { get; set; }
    }
}