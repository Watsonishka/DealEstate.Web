using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Bids;

namespace OnlineShop.DB.Storages
{
    public class ServicesDBStorage : IServicesStorage
    {
        private readonly DatabaseContext _databaseContext;

        public ServicesDBStorage(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Service> GetAll() => _databaseContext.Services.ToList();
    }
}