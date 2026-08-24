using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Bids;

namespace OnlineShop.DB.Storages
{
    public class CatalogServicesDBStorage: ICatalogServicesStorage
    {
        private readonly DatabaseContext _databaseContext;
        public CatalogServicesDBStorage(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<CatalogService> GetAll() => _databaseContext.CatalogServices.ToList();
        
    }
}
