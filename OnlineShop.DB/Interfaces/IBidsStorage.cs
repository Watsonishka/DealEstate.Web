using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Interfaces
{
    public interface IBidsStorage
    {
        Bid? TryGetByUserId(string? userId);
        Bid? TryGetByUserOrAnonymous(string? userId, string? anonymousId);
        void Clear(string userId);
        void Add(Product product, string? userId, string? anonymousId);
        void Remove(Product product, string userId, string anonymousId);
        void ToggleService(Guid serviceId);
        void Merge(string anonymousId, string userId);
    }
}
