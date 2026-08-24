using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsStorage _productsStorage;
        public ProductController(IProductsStorage productsStorage)
        {
            _productsStorage = productsStorage;
        }

        public IActionResult Index(Guid id)
        {
            var product = _productsStorage.TryGetByID(id);

            return View(product.ToProductViewModel());
        }
    }
}
