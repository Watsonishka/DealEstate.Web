using OnlineShop.DB.Models.Bids;

namespace OnlineShop.DB.Interfaces
{
    public interface ICatalogServicesStorage
    {
        List<CatalogService> GetAll();
    }
}
