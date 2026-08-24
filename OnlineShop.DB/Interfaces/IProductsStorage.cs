using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Interfaces
{
    public interface IProductsStorage
    {
        List<Product> GetAll();
        List<Product> GetAllApartments();
        List<Product> GetAllHouses();
        Product? TryGetByID(Guid productID);
        void Add(Product product);
        void Delete(Guid productID);
        void Update(Product product);
        List<Product> Search(string query);
    }
}
