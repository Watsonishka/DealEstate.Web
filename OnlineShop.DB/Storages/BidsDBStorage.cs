using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Storages
{
    public class BidsDBStorage : IBidsStorage
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ICatalogServicesStorage _catalogServicesStorage;
        public BidsDBStorage(DatabaseContext databaseContext, ICatalogServicesStorage catalogServicesStorage)
        {
            _databaseContext = databaseContext;
            _catalogServicesStorage = catalogServicesStorage;
        }

        public Bid? TryGetByUserOrAnonymous(string? userId, string? anonymousId)
        {
            if (userId != null)
            {
                return TryGetByUserId(userId);
            }

            return TryGetByAnonymousId(anonymousId);
        }

        public Bid? TryGetByUserId(string userId)
        {
            return _databaseContext.Bids.Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.Items)
                .ThenInclude(i => i.Services)
                .FirstOrDefault(bid => bid.User.Id == userId);
        }

        public void Clear(string userId)
        {
            var existingBid = TryGetByUserId(userId);

            if (existingBid != null)
            {
                _databaseContext.Bids.Remove(existingBid);
                _databaseContext.SaveChanges();
            }
        }

        public void Add(Product product, string? userId, string? anonymousId)
        {
            var existingBid = new Bid();

            if (!string.IsNullOrEmpty(userId))
            {
                existingBid = TryGetByUserId(userId);
            }
            else
            {
                existingBid = TryGetByAnonymousId(anonymousId);
            }

            if (existingBid == null)
            {
                existingBid = new Bid
                {
                    UserID = userId,
                    AnonymusID = anonymousId,
                    Items = []
                };
                _databaseContext.Bids.Add(existingBid);
                _databaseContext.SaveChanges();
            }

            var existingItem = existingBid.Items.FirstOrDefault(item => item.Product.ID == product.ID);

            if (existingItem == null)
            {
                var newItem = AddNewItem(product);
                _databaseContext.BidItems.Add(newItem);
                existingBid.Items.Add(newItem);
                _databaseContext.SaveChanges();
            }
        }

        public void Remove(Product product, string userId, string anonymousId)
        {
            var existingBid = TryGetByUserOrAnonymous(userId, anonymousId);
            if (existingBid == null)
            {
                return;
            }

            var existingItem = existingBid.Items.FirstOrDefault(x => x.Product.ID == product.ID);
            if (existingItem != null)
            {
                _databaseContext.BidItems.Remove(existingItem);
                _databaseContext.SaveChanges();
            }
        }

        public void ToggleService(Guid serviceID)
        {
            var service = _databaseContext.Services.Find(serviceID);
            service.IsRemoved = !service.IsRemoved;
            _databaseContext.SaveChanges();
        }

        private BidItem AddNewItem(Product product)
        {
            var catalog = _catalogServicesStorage.GetAll();

            var bidItem = new BidItem
            {
                Product = product
            };

            bidItem.Services = catalog.Select(catalogService => new Service
            {
                Name = catalogService.Name,
                Price = catalogService.Price,
                BidItem = bidItem,
                IsRemoved = false
            }).ToList();

            return bidItem;
        }

        public void Merge(string anonymousId, string userId)
        {
            var anonymousBid = TryGetByAnonymousId(anonymousId);

            if (anonymousBid == null)
            {
                return;
            }

            var userBid = TryGetByUserId(userId);

            if (userBid == null)
            {
                anonymousBid.UserID = userId;
                anonymousBid.AnonymusID = null;
                _databaseContext.SaveChanges();
                return;
            }

            foreach (var anonymousItem in anonymousBid.Items.ToList())
            {
                var existingItem = userBid.Items
                    .FirstOrDefault(x => x.Product.ID == anonymousItem.Product.ID);

                if (existingItem == null)
                {
                    anonymousItem.Bid = userBid;
                }
                else
                {
                    _databaseContext.BidItems.Remove(anonymousItem);
                }
            }

            _databaseContext.Bids.Remove(anonymousBid);
            _databaseContext.SaveChanges();
        }

        private Bid? TryGetByAnonymousId(string? anonymousId)
        {
            return _databaseContext.Bids
                .Include(b => b.Items)
                .ThenInclude(i => i.Product)
                .Include(b => b.Items)
                .ThenInclude(i => i.Services)
                .FirstOrDefault(bid => bid.AnonymusID == anonymousId);
        }
    }
}
