using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShopWebApp.Helpers;
namespace OnlineShopWebApp.Controllers;

public class HomeController : Controller
{
    private readonly IProductsStorage _productsStorage;
    public HomeController(IProductsStorage productsStorage)
    {
        _productsStorage = productsStorage;
    }

    public IActionResult Index()
    {
        var products = _productsStorage.GetAll();
        return View(products.ToProductViewModels());
    }

    public IActionResult Search(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return View("Index", _productsStorage.GetAll().ToProductViewModels());
        }
        return View("Index", _productsStorage.Search(query).ToProductViewModels());
    }
}
