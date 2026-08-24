using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Products;

namespace OnlineShop.DB.Storages
{
    public class ProductsDBStorage : IProductsStorage
    {
        private readonly string _apartmentDefaultPreviewFilePath = "/img/Apartment_white.png";
        private readonly string _houseDefaultPreviewFilePath = "/img/House_white.png";
        private readonly DatabaseContext _databaseContext;

        public ProductsDBStorage(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Product> GetAll() => _databaseContext.Products.ToList();

        public List<Product> GetAllApartments()
        {
            return _databaseContext.Products.Where(product => product.Category == Category.Apartments).ToList();
        }

        public List<Product> GetAllHouses()
        {
            return _databaseContext.Products.Where(product => product.Category == Category.Houses).ToList();
        }

        public Product? TryGetByID(Guid productID)
        {
            return _databaseContext.Products.FirstOrDefault(product => product.ID == productID);
        }

        public void Add(Product product)
        {
            if (product.PreviewImagePath == null)
            {
                product.PreviewImagePath = product.Category == Category.Apartments ? _apartmentDefaultPreviewFilePath : _houseDefaultPreviewFilePath;
            }

            _databaseContext.Add(product);

            _databaseContext.SaveChanges();
        }

        public void Delete(Guid productID)
        {
            var productToDelete = TryGetByID(productID);

            if (productToDelete != null)
            {
                var bidItems = _databaseContext.BidItems
                       .Include(services => services.Services)
                       .Where(product => product.Product.ID == productID)
                       .ToList();

                foreach (var bidItem in bidItems)
                {
                    _databaseContext.Services.RemoveRange(bidItem.Services);
                }

                _databaseContext.BidItems.RemoveRange(bidItems);
                _databaseContext.Products.Remove(productToDelete);

                _databaseContext.SaveChanges();
            }
        }
        public void Update(Product product) 
        {
            var existingProduct = TryGetByID(product.ID);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Cost = product.Cost;
                existingProduct.Area = product.Area;
                existingProduct.Description = product.Description;
                existingProduct.TotalFloors = product.TotalFloors;
                existingProduct.Developer = product.Developer;
                existingProduct.City = product.City;
                existingProduct.Class = product.Class;
                existingProduct.PreviewImagePath = product.PreviewImagePath;

                if (product is Apartment apartment && existingProduct is Apartment existingApartment)
                {
                    existingApartment.Floor = apartment.Floor;
                    existingApartment.HasBalcony = apartment.HasBalcony;
                    existingApartment.CeilingHeight = apartment.CeilingHeight;
                }
                if (product is House house && existingProduct is House existingHouse)
                {
                    existingHouse.LandArea = house.LandArea;
                    existingHouse.HasGarage = house.HasGarage;
                }

                _databaseContext.SaveChanges();
            }
        }

        public List<Product> Search(string query) => _databaseContext.Products.Where(
            product => product.Name.ToLower().Contains(query) ||
            product.Developer.ToLower().Contains(query) ||
            product.City.ToLower().Contains(query) ||
            product.Area.ToString().Contains(query)).ToList();
    }
}
