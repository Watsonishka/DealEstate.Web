using OnlineShop.DB.Models.Products;

namespace OnlineShopWebApp.Models
{
    public class BidItemViewModel
    {
        public Guid ID { get; set; }
        public Product Product { get; set; }
        public List<ServiceViewModel> Services { get; set; } 
    }
}
