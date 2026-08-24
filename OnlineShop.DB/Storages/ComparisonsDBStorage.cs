using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Comparisons;
using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Storages
{
    public class ComparisonsDBStorage : IComparisonsStorage
    {
        private readonly DatabaseContext _databaseContext;
        public ComparisonsDBStorage(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Comparison? TryGetByUserID(string userID)
        {
            return _databaseContext.Comparisons.Include(products => products.Products)
                .FirstOrDefault(id => id.UserID == userID);
        }

        public void Add(Product product, string userID)
        {
            var existingComparison = TryGetByUserID(userID);

            if (existingComparison == null)
            {
                existingComparison = new Comparison()
                {
                    UserID = userID,
                    Products = [product]
                };

                _databaseContext.Comparisons.Add(existingComparison);
                _databaseContext.SaveChanges();
            }
            else
            {
                var existingComparisonItem = existingComparison.Products.FirstOrDefault(x => x.ID == product.ID);

                if (existingComparisonItem == null)
                {
                    existingComparison.Products.Add(product);
                    _databaseContext.SaveChanges();
                }
            }
        }

        public void Remove(Guid productID, string userID)
        {
            var existingComparison = TryGetByUserID(userID);

            if (existingComparison == null)
            {
                return;
            }

            var existingComparisonItem = existingComparison.Products.FirstOrDefault(x => x.ID == productID);

            if (existingComparisonItem != null)
            {
                existingComparison.Products.Remove(existingComparisonItem);
                _databaseContext.SaveChanges();
            }

        }

        public void Clear(string userId)
        {
            var existingComparison = TryGetByUserID(userId);

            if (existingComparison != null)
            {
                existingComparison.Products.Clear();
                _databaseContext.SaveChanges();
            }
        }
    }
}
