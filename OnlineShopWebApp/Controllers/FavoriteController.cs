using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Users;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoritesStorage _favoritesStorage;
        private readonly IProductsStorage _productsStorage;
        private readonly UserManager<User> _userManager;


        public FavoriteController(IFavoritesStorage favoritesStorage, IProductsStorage productsStorage, UserManager<User> userManager)
        {
            _favoritesStorage = favoritesStorage;
            _productsStorage = productsStorage;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUserID = _userManager.GetUserId(User);
            var favorite = _favoritesStorage.TryGetByUserId(currentUserID);
            return View(favorite.ToFavoriteViewModel());
        }

        public IActionResult Add(Guid productId)
        {
            var currentUserID = _userManager.GetUserId(User);
            var product = _productsStorage.TryGetByID(productId);


            if (product != null)
            {
                _favoritesStorage.Add(product, currentUserID);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(Guid productId)
        {
            var currentUserID = _userManager.GetUserId(User);
            var product = _productsStorage.TryGetByID(productId);

            if (product != null)
            {
                _favoritesStorage.Remove(product, currentUserID);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
