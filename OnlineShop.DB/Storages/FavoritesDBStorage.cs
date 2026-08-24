using Microsoft.EntityFrameworkCore;
using OnlineShop.DB;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Favorites;
using OnlineShop.DB.Models.Products;

namespace OOnlineShop.DB.Storages
{
    public class FavoritesDBStorage : IFavoritesStorage
    {
        private readonly DatabaseContext _databaseContext;

        public FavoritesDBStorage(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Favorite? TryGetByUserId(string userId)
        {
            return _databaseContext.Favorites.Include(products => products.Products)
                .FirstOrDefault(id => id.UserID == userId);
        }

        public void Add(Product product, string userId)
        {
            var existingFavorite = TryGetByUserId(userId);

            if (existingFavorite == null)
            {
                existingFavorite = new Favorite()
                {
                    UserID = userId,
                    Products = new List<Product> { product }
                };

                _databaseContext.Favorites.Add(existingFavorite);
                _databaseContext.SaveChanges();

            }
            else
            {
                var existingFavoriteItem = existingFavorite.Products.FirstOrDefault(item => item.ID == product.ID);

                if (existingFavoriteItem == null)
                {
                    existingFavorite.Products.Add(product);
                    _databaseContext.SaveChanges();
                }
            }
        }

        public void Remove(Product product, string userId)
        {
            var existingFavorite = TryGetByUserId(userId);

            if (existingFavorite == null)
            {
                return;
            }

            var existingItem = existingFavorite.Products.FirstOrDefault(item => item.ID == product.ID);

            if (existingItem != null)
            {
                existingFavorite.Products.Remove(existingItem);
                _databaseContext.SaveChanges();
            }
        }

        public void Clear(string userId)
        {
            var existingFavorite = TryGetByUserId(userId);

            if (existingFavorite != null)
            {
                _databaseContext.Favorites.Remove(existingFavorite);
                _databaseContext.SaveChanges();
            }
        }
    }
}
