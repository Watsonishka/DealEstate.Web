using OnlineShop.DB.Models.Comparisons;
using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Interfaces
{
    public interface IComparisonsStorage
    {
        Comparison? TryGetByUserID(string userID);
        void Add(Product product, string userID);
        void Remove(Guid productID, string userID);
        void Clear(string userId);
    }
}
