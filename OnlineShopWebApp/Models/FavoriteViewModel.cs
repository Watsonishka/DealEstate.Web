using OnlineShop.DB.Models.Products;

namespace OnlineShopWebApp.Models
{
    public class FavoriteViewModel
    {
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public List<Product> Products { get; set; } 
    }
}
