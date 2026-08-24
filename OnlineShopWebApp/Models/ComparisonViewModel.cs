using OnlineShop.DB.Models.Products;

namespace OnlineShopWebApp.Models
{
    public class ComparisonViewModel
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public List<Product> Products { get; set; }
    }
}
