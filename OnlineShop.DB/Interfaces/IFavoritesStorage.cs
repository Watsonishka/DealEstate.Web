using OnlineShop.DB.Models.Products;
using OnlineShop.DB.Models.Favorites;

namespace OnlineShop.DB.Interfaces
{
    public interface IFavoritesStorage
    {
        Favorite? TryGetByUserId(string userId);
        void Add(Product product, string userId);
        void Remove(Product product, string userId);
        void Clear(string userId);
    }
}
