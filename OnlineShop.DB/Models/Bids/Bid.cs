using OnlineShop.DB.Models.Users;

namespace OnlineShop.DB.Models.Bids
{
    public class Bid
    {
        public Guid ID { get; set; }
        public string? AnonymusID { get; set; }
        public string? UserID { get; set; }
        public List<BidItem> Items { get; set; }
        public User User { get; set; }
    }
}
