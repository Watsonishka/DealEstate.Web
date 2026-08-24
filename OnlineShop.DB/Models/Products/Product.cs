using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Favorites;
using OnlineShop.DB.Models.Comparisons;
using OnlineShop.DB.Models.Orders;

namespace OnlineShop.DB.Models.Products
{
    public abstract class Product
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public double Area { get; set; }

        public string? Description { get; set; }

        public int TotalFloors { get; set; }

        public Category Category { get; set; }

        public string Developer { get; set; }

        public string City { get; set; }

        public string? PreviewImagePath { get; set; }

        public ApartmentClass Class { get; set; }

        public List<BidItem>? BidItems { get; set; }

        public List<Favorite> Favorites { get; set; } 

        public List<Comparison> Comparisons { get; set; }

        public List<OrderItem> OrderItems { get; set; } 
    }
}