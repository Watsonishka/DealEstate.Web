using OnlineShop.DB.Models.Bids;

namespace OnlineShop.DB.Interfaces
{
    public interface IServicesStorage
    {
        List<Service> GetAll();
    }
}
