using OnlineShop.DB.Models.Products;

namespace OnlineShopWebApp.Models
{
    public class OrderItemViewModel
    {
        public Guid ID { get; set; }
        public Product? Product { get; set; }
        public string ProductName { get; set; }
        public string ProductDeveloper { get; set; }
        public string ProductCity { get; set; }
        public double ProductArea { get; set; }
        public decimal CurrentPrice { get; set; }
        public List<ServiceViewModel> Services { get; set; } = [];
    }
}
