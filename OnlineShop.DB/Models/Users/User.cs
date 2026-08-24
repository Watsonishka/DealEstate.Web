using Microsoft.AspNetCore.Identity;
using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Comparisons;
using OnlineShop.DB.Models.Favorites;
using OnlineShop.DB.Models.Orders;

namespace OnlineShop.DB.Models.Users
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public bool IsCancelled { get; set; } 
        public List<Comparison>? Comparisons { get; set; }
        public List<Favorite>? Favorites { get; set; }
        public List<Order>? Orders { get; set; }
        public Bid? Bid { get; set; }
    }
}
