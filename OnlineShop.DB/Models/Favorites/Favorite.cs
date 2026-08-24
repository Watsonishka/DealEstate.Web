using OnlineShop.DB.Models.Products;
using OnlineShop.DB.Models.Users;

namespace OnlineShop.DB.Models.Favorites
{
    public class Favorite
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public List<Product> Products { get; set; }
        public User User { get; set; }
    }
}
